using MediatR;
using Vendora.Services.Catalog.Api.Constants;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.Update;

public static class UpdateProductEndpoint
{
    public static void MapUpdateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut(UpdateProductRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageProducts);
    }

    private static async Task<IResult> Handle(
        UpdateProductRequest request, 
        ISender sender, 
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand();

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}