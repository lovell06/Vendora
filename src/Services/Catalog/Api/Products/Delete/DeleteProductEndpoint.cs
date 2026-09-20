using MediatR;
using Vendora.Services.Catalog.Api.Constants;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.Delete;

public static class DeleteProductEndpoint
{
    public static void MapDeleteProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete(DeleteProductRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageProducts);
    }

    private static async Task<IResult> Handle(
        [AsParameters] DeleteProductRequest request,
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