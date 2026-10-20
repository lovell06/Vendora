namespace Vendora.Services.Inventory.Api.InventoryItems.Reserve;

public static class ReserveInventoryItemsEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapReserveInventoryItemEndpoint()
        {
            group.MapPost(ReserveInventoryItemsRequest.Pattern, Handle);
        }
    }

    public static async Task<IResult> Handle(
        ReserveInventoryItemsRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand();

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();
        
        return Results.NoContent();
    }
}