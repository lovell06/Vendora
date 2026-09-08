using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Vendora.Services.Identity.Application.Abstractions.Authentication;
using Vendora.Services.Identity.Domain.Users;
using Vendora.Services.Identity.Infrastructure.Options;

namespace Vendora.Services.Identity.Infrastructure.Authentication;

public class JsonWebTokenProvider(
    IOptions<JwtOptions> options,
    TimeProvider clock) : IAccessTokenProvider
{
    private readonly JwtOptions _jwt = options.Value;
    public string Issue(User user)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new ("role", user.Role.ToString())
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwt.Issuer,
            Audience = _jwt.Audience,
            IssuedAt = utcNow,
            NotBefore = utcNow,
            Subject = new ClaimsIdentity(claims),
            Expires = utcNow.AddMinutes(_jwt.ExpireMinutes),
            SigningCredentials = credential
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}