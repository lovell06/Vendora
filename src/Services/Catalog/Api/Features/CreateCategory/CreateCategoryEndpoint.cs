using MediatR;
using Vendora.Services.Catalog.Api.Constants;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Features.CreateCategory;

public static class CreateCategoryEndpoint
{
    public static void MapCreateCategoryEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/create", Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageCategories);
    }

    public static async Task<IResult> Handle(
        CreateCategoryRequest request, 
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