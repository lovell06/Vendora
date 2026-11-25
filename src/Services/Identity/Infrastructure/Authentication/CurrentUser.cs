using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Vendora.Services.Identity.Infrastructure.Authentication;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private readonly HttpContext _context = accessor.HttpContext
                                            ?? throw new InvalidOperationException("No current HTTP request.");
    
    public Guid UserId => GetUserId();

    public async Task<string> GetAccessTokenAsync()
    {
        var token = await _context.GetTokenAsync("access_token");

        return !string.IsNullOrWhiteSpace(token)
            ? token
            : throw new InvalidOperationException("Bearer token is empty.");
    }

    public Guid GetAccessTokenId()
    {
        var raw = _context.User.FindFirstValue(JwtRegisteredClaimNames.Jti)
            ?? throw new InvalidOperationException("Missing or invalid JTI claim.");

        var accessTokenId = Guid.Parse(raw);

        return accessTokenId != Guid.Empty
            ? accessTokenId
            : throw new InvalidOperationException("Missing or invalid JTI claim.");
    }

    public DateTimeOffset GetExpirationDateTime()
    {
        var raw = _context.User.FindFirstValue(JwtRegisteredClaimNames.Exp)
                  ?? throw new InvalidOperationException("Missing or invalid EXP claim.");

        if (!long.TryParse(raw, out var exp))
            throw new InvalidOperationException("Invalid Exp claim format");

        return DateTimeOffset.FromUnixTimeSeconds(exp);
    }

    private Guid GetUserId()
    {
        var raw = _context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                        ?? throw new InvalidOperationException("Missing or invalid UserID claim.");

        var userId = Guid.Parse(raw);

        return userId != Guid.Empty
            ? userId
            : throw new InvalidOperationException("Missing or invalid UserID claim.");
    }
}