namespace Vendora.Services.Cart.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var issuer = configuration["Jwt:Issuer"]
                             ?? throw new InvalidOperationException("Missing Jwt:Issuer configuration.");

                var audience = configuration["Jwt:Audience"]
                               ?? throw new InvalidOperationException("Missing Jwt:Audience configuration.");

                var publicKeyPath = configuration["Jwt:PublicKeyPath"]
                                    ?? throw new InvalidOperationException("Missing Jwt:PublicKeyPath configuration.");

                RSAParameters parameters;

                using (var rsa = RSA.Create())
                {
                    rsa.ImportFromPem(File.ReadAllText(publicKeyPath));
                    parameters = rsa.ExportParameters(includePrivateParameters: false);
                }

                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(parameters),

                    ValidAlgorithms = [ SecurityAlgorithms.RsaSha256 ],

                    NameClaimType = JwtRegisteredClaimNames.Sub,
                    RoleClaimType = "role"
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}