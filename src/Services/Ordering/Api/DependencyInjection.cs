namespace Vendora.Services.Ordering.Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJwtBearerAuthentication(IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var issuer = configuration["Jwt:Issuer"]
                             ?? throw new InvalidOperationException("Missing Jwt:Issuer Configuration.");

                var audience = configuration["Jwt:Audience"]
                               ?? throw new InvalidOperationException("Missing Jwt:Audience Configuration.");

                var publicKeyPath = configuration["Jwt:PublicKeyPath"]
                                    ?? throw new InvalidOperationException("Missing Jwt:PublicKeyPath Configuration.");
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
            return services;
        }

        public IServiceCollection AddApplicationAuthorization()
        {
            services.AddAuthorization();
            return services;
        }
    }
}