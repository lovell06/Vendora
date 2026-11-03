using Vendora.Services.Identity.Api.Users.CreateEmployeeUser;

namespace Vendora.Services.Identity.Api.Users;

public static class GroupMapper
{
    extension(RouteGroupBuilder api)
    {
        public void MapUserEndpoints()
        {
            var users = api.MapGroup("/users").WithTags("Users");

            users.MapCreateEmployeeUserEndpoint();
        }
    }
}