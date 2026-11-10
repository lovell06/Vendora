namespace Vendora.Services.Ordering.Application.Orders.Place;

public sealed class Handler(
    IOrderRepository orderRepository,
    IOutboxMessageRepository outboxMessageRepository,
    IUnitOfWork unitOfWork,
    ICartServiceClient cartServiceClient,
    ICatalogServiceClient catalogServiceClient,
    IInventoryServiceClient inventoryServiceClient,
    TimeProvider clock,
    ILogger<Handler> logger) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var itemsResult = await cartServiceClient.GetSelectedItems(
            productVariantIds: cmd.ProductVariantIds,
            ct: cancellationToken);

        if (itemsResult.IsFailure)
            return Result.Failure(itemsResult.Error);

        var cartItemsResponse = itemsResult.Value;

        var productVariantsInfoResult = await catalogServiceClient.GetProductVariantsInfoAsync(
            productVariantIds: [.. cartItemsResponse.Items.Select(i => i.ProductVariantId)],
            ct: cancellationToken);

        if (productVariantsInfoResult.IsFailure)
            return Result.Failure(productVariantsInfoResult.Error);

        if (productVariantsInfoResult.Value.ProductVariants.Count == 0)
            return Result.Failure(Errors.ProductNotFound);
        
        var productVariantsInfo = productVariantsInfoResult.Value.ProductVariants.ToDictionary(p => p.VariantId);

        var currencyTypeCount = productVariantsInfo.Select(p => p.Value.Currency).Distinct().Count();
        if (currencyTypeCount != 1)
        {
            logger.LogWarning(
                "Placing order rejected because order has more than one currency type. ({count})",
                currencyTypeCount);

            return Result.Failure(Errors.InvalidCurrencyTypeCount);
        }

        var orderItems = new List<OrderItem>();
        foreach (var item in cartItemsResponse.Items)
        {
            if (!productVariantsInfo.TryGetValue(item.ProductVariantId, out var productVariantInfo))
            {
                logger.LogWarning(
                    "Placing order rejected because product {Id} not found.",
                    item.ProductVariantId);

                return Result.Failure(Errors.ProductNotFound);
            }

            var orderItemResult = OrderItem.Create(
                variantId: item.ProductVariantId,
                productId: productVariantInfo.ProductId,
                sku: productVariantInfo.Sku,
                productName: productVariantInfo.ProductName,
                variantName: productVariantInfo.VariantName,
                quantity: item.Quantity,
                unitPrice: productVariantInfo.Price);

            if (orderItemResult.IsFailure)
            {
                return Result.Failure(orderItemResult.Error);
            }

            orderItems.Add(orderItemResult.Value);
        }

        var orderResult = Order.Place(
            userId: cmd.UserId,
            items: orderItems,
            currency: productVariantsInfo.Values.ToArray()[0].Currency,
            createdAt: utcNow);

        if (orderResult.IsFailure)
        {
            logger.LogWarning(
                "Placing order rejected because {error}",
                orderResult.Error.Code);

            return Result.Failure(orderResult.Error);
        }

        var order = orderResult.Value;

        orderRepository.Add(order);

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var reservedResult = await inventoryServiceClient.ReserveAsync(
                orderItems: order.Items, 
                ct: cancellationToken);

            if (reservedResult.IsFailure)
            {
                var rejectedResult = order.Reject(utcNow);
                if (rejectedResult.IsFailure)
                    throw new InvalidOperationException(rejectedResult.Error.Message);
                
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Failure(reservedResult.Error);
            }

            var confirmedResult = order.Confirm(utcNow);
            if (confirmedResult.IsFailure)
                throw new InvalidOperationException(confirmedResult.Error.Message);

            outboxMessageRepository.Add(new OrderPlacedEvent
            {
                Id = Guid.CreateVersion7(),
                OccurredAt = utcNow,
                OrderId = order.Id,
                UserId = order.UserId,
                Items = [.. order.Items.Select(i => i.ProductId)]
            }, utcNow);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
        
        return Result.Success();
    }
}
