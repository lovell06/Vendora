namespace Vendora.Services.Cart.Infrastructure.Authentication;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private readonly HttpContext _context = accessor.HttpContext
                                            ?? throw new InvalidOperationException("No current HTTP request.");

    public Guid UserId => Guid.Parse(GetUserIdRaw());
    
    private string GetUserIdRaw()
    {
        var sub = _context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (sub is null)
        {
            var error = new StringBuilder();
            error.AppendLine($"Not found: {JwtRegisteredClaimNames.Sub}");
            error.AppendLine("System only contains: ");
            foreach (var claim in _context.User.Claims)
            {
                error.AppendLine($"{nameof(claim.Type)}: {claim.Value};");
            }

            throw new InvalidOperationException(error.ToString());
        }

        return sub;
    }
    
    public async Task<string> GetAccessTokenAsync()
    {
        var token = await _context.GetTokenAsync("access_token");

        return !string.IsNullOrWhiteSpace(token)
            ? token
            : throw new InvalidOperationException("Bearer token is empty.");
    }
}