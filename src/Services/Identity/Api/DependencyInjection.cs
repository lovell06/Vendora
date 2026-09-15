using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Vendora.Services.Identity.Infrastructure.Options;

namespace Vendora.Services.Identity.Api;

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
            var jwtOptions = configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>() ?? throw new InvalidOperationException("Jwt is not configured.");

            RSAParameters parameters;
            using (var rsa = RSA.Create())
            {
                rsa.ImportFromPem(jwtOptions.PrivateKeyPem);
                parameters = rsa.ExportParameters(includePrivateParameters: false);
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(parameters),

                ValidAlgorithms = [SecurityAlgorithms.RsaSha256]
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                    logger.LogWarning(context.Exception, "JWT validation failed.");
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();
        return services;
    }
}