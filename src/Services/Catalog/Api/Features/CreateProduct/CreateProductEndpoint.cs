using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Features.CreateProduct;

public static class CreateProductEndpoint
{
    public static void MapCreateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/products/create", Handle);
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