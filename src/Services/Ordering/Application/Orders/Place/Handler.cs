using Microsoft.Extensions.Logging;
using Vendora.Services.Ordering.Application.Abstractions.Clients;
using Vendora.Services.Ordering.Application.Abstractions.Persistence;
using Vendora.Services.Ordering.Domain.Orders;

namespace Vendora.Services.Ordering.Application.Orders.Place;

public sealed class Handler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    ICartServiceClient cartServiceClient,
    ICatalogServiceClient catalogServiceClient,
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

        var order = Order.Place(
            userId: cmd.UserId,
            items: orderItems,
            currency: productsInfo.Values.ToArray()[0].Currency,
            createdAt: utcNow);

        if (order.IsFailure)
        {
            logger.LogWarning(
                "Placing order rejected because {error}",
                order.Error.Code);

            return Result.Failure(order.Error);
        }

        orderRepository.Add(order.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
