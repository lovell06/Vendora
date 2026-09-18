using MediatR;
using Vendora.Services.Catalog.Api.Constants;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.Create;

public static class CreateProductEndpoint
{
    public static void MapCreateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost(CreateProductRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageProducts);
    }

    private static async Task<IResult> Handle(
        CreateProductRequest request, 
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