using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var connString = builder.Configuration
                         .GetConnectionString("Redis")
                     ?? throw new InvalidOperationException("Redis connection is not configured.");

    return ConnectionMultiplexer.Connect(connString);
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var issuer = builder.Configuration["Jwt:Issuer"]
                     ?? throw new InvalidOperationException("Missing Jwt:Issuer configuration.");

        var audience = builder.Configuration["Jwt:Audience"]
                       ?? throw new InvalidOperationException("Missing Jwt:Audience configuration.");

        var publicKeyPath = builder.Configuration["Jwt:PublicKeyPath"]
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

            ValidAlgorithms = [SecurityAlgorithms.RsaSha256],

            NameClaimType = JwtRegisteredClaimNames.Sub,
            RoleClaimType = "role"
        };

        options.Events = new JwtBearerEvents()
        {
            OnTokenValidated = async ctx =>
            {
                var jti = ctx.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
                var redis = ctx.HttpContext.RequestServices.GetRequiredService<IConnectionMultiplexer>();

                if (jti is null ||
                    await redis.GetDatabase().KeyExistsAsync($"identity:access-token:black-list:{jti}"))
                    ctx.Fail("token_revoked");
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.AddDocument("identity", "Identity", "/openapi/identity.json")
            .AddDocument("catalog", "Catalog", "/openapi/catalog.json")
            .AddDocument("inventory", "Inventory", "/openapi/inventory.json")
            .AddDocument("cart", "Cart", "/openapi/cart.json")
            .AddDocument("ordering", "Ordering", "/openapi/ordering.json");
    });
}

app.MapHealthChecks("/health");

app.UseAuthentication();

app.Use(async (ctx, next) =>
{
    var hasBearer = ctx.Request.Headers.Authorization.ToString()
        .StartsWith("Bearer", StringComparison.OrdinalIgnoreCase);

    if (hasBearer && ctx.User.Identity?.IsAuthenticated != true)
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    await next();
});

app.MapReverseProxy();

app.Run();
