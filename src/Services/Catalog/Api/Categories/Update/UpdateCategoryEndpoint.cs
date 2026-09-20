using MediatR;
using Vendora.Services.Catalog.Api.Constants;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Categories.Update;

public static class UpdateCategoryEndpoint
{
    public static void MapUpdateCategoryEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut(UpdateCategoryRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageCategories);
    }

    private static async Task<IResult> Handle(
        UpdateCategoryRequest request,
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