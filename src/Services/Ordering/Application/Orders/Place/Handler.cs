namespace Vendora.Services.Ordering.Application.Orders.Place;

public sealed class Handler(
    IOrderRepository orderRepository,
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
            productIds: cmd.ProductIds,
            ct: cancellationToken);

        if (itemsResult.IsFailure)
            return Result.Failure(itemsResult.Error);

        var cartItemsResponse = itemsResult.Value;

        var productsInfoResult = await catalogServiceClient.GetProductsInfoAsync(
            productIds: [.. cartItemsResponse.Items.Select(i => i.ProductId)],
            ct: cancellationToken);

        if (productsInfoResult.IsFailure)
            return Result.Failure(productsInfoResult.Error);

        if (productsInfoResult.Value.Products.Count == 0)
            return Result.Failure(Errors.ProductNotFound);
        
        var productsInfo = productsInfoResult.Value.Products.ToDictionary(p => p.Id);

        var currencyTypeCount = productsInfo.Select(p => p.Value.Currency).Distinct().Count();
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
            if (!productsInfo.TryGetValue(item.ProductId, out var productInfo))
            {
                logger.LogWarning(
                    "Placing order rejected because product {Id} not found.",
                    item.ProductId);

                return Result.Failure(Errors.ProductNotFound);
            }

            var orderItemResult = OrderItem.Create(
                productId: item.ProductId,
                productName: productInfo.Name,
                quantity: item.Quantity,
                unitPrice: productInfo.Price);

            if (orderItemResult.IsFailure)
            {
                return Result.Failure(orderItemResult.Error);
            }

            orderItems.Add(orderItemResult.Value);
        }

        var orderResult = Order.Place(
            userId: cmd.UserId,
            items: orderItems,
            currency: productsInfo.Values.ToArray()[0].Currency,
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
