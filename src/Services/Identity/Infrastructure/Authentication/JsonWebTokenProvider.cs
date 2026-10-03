namespace Vendora.Services.Identity.Infrastructure.Authentication;

public class JsonWebTokenProvider(
    IOptions<JwtOptions> options,
    TimeProvider clock) : IAccessTokenProvider
{
    private readonly JwtOptions _jwt = options.Value;
    public string Issue(User user)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        RSAParameters parameters;
        using (var rsa = RSA.Create())
        {
            rsa.ImportFromPem(File.ReadAllText(_jwt.PrivateKeyPath));
            parameters = rsa.ExportParameters(includePrivateParameters: true);
        }

        var key = new RsaSecurityKey(parameters);
        var credential = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
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