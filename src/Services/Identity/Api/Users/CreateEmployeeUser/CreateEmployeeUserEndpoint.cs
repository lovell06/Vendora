using Vendora.Services.Identity.Api.Constants;

namespace Vendora.Services.Identity.Api.Users.CreateEmployeeUser;

public static class CreateEmployeeUserEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapCreateEmployeeUserEndpoint()
        {
            group.MapPost(CreateEmployeeUserRequest.Pattern, Handle)
                .RequireAuthorization(AuthorizationPolicies.ManageAccounts);
        }
    }

    private static async Task<IResult> Handle(
        CreateEmployeeUserRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand();

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Created();
    }
}