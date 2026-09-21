using Vendora.Services.Inventory.Api.InventoryItems;

namespace Vendora.Services.Inventory.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        api.MapInventoryItems();
        
        return app;
    }
}