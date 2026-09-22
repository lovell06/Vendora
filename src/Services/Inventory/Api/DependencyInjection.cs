using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Vendora.Services.Inventory.Api.Constants;

namespace Vendora.Services.Inventory.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var issuer = configuration.GetValue<string>("Jwt:Issuer")
                         ?? throw new InvalidOperationException("Missing Jwt:Issuer Configuration.");

            var audience = configuration.GetValue<string>("Jwt:Audience")
                           ?? throw new InvalidOperationException("Missing Jwt:Audience Configuration.");

            var publicKeyPath = configuration.GetValue<string>("Jwt:PublicKeyPath")
                                ?? throw new InvalidOperationException("Missing Jwt:PublicKeyPath Configuration.");

            RSAParameters parameters;
            using (var rsa = RSA.Create())
            {
                rsa.ImportFromPem(File.ReadAllText(publicKeyPath));
                parameters = rsa.ExportParameters(includePrivateParameters: false);
            }

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(parameters),

                ValidAlgorithms = [ SecurityAlgorithms.RsaSha256 ]
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.ManageStock, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(RoleNames.Admin);
            });
        });
        
        return services;
    }
}