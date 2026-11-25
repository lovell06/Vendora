using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Vendora.Web.Authentication;

// Web has no HTTP authentication scheme: the session lives in ProtectedSessionStorage and is only readable
// inside the circuit. Let the request through and let AuthorizeRouteView decide when it renders.
public sealed class PassThroughAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next, HttpContext context,
        AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        => next(context);
}
