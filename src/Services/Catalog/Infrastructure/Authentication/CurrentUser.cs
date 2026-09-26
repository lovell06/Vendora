using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Vendora.Services.Catalog.Application.Abstractions.Authentication;

namespace Vendora.Services.Catalog.Infrastructure.Authentication;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private readonly HttpContext _context = accessor.HttpContext
                                            ?? throw new InvalidOperationException("No current HTTP request.");

    public Guid UserId => Guid.Parse(_context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                                     ?? throw new InvalidOperationException(
                                         "Missing or invalid user ID claim."));

    public async Task<string> GetAccessTokenAsync()
    {
        var token = await _context.GetTokenAsync("access_token");

        return !string.IsNullOrWhiteSpace(token)
            ? token
            : throw new InvalidOperationException("Bearer token is empty.");
    }
}