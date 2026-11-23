using Microsoft.AspNetCore.Mvc;

namespace Vendora.Services.Catalog.Api.Products.Create;

public static class CreateProductEndpoint
{
    public static void MapCreateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost(CreateProductRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageProducts)
            .DisableAntiforgery();
    }

    private static async Task<IResult> Handle(
        [FromForm] CreateProductRequest request, 
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Created();
    }
}