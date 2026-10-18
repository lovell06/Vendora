namespace Vendora.Services.Ordering.Infrastructure.Authentication;

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

    private Guid GetUserId()
    {
        var rawUserId = _context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                        ?? throw new InvalidOperationException("Missing or invalid UserID claim.");

        var userId = Guid.Parse(rawUserId);

        return userId != Guid.Empty
            ? userId
            : throw new InvalidOperationException("Missing or invalid UserID claim.");
    }
}