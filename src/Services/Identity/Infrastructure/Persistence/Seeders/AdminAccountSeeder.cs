using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vendora.Services.Identity.Application.Abstractions.Authentication;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Infrastructure.Persistence.Seeders;

public static class AdminAccountSeeder
{
    public static void Seed(DbContext context, IServiceProvider provider, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("AdminSeedingEnable"))
            return;
                
        var utcNow = provider.GetRequiredService<TimeProvider>().GetUtcNow().UtcDateTime;

        var adminAccount = configuration.GetRequiredSection("AdminAccount");

        var email = adminAccount["Email"]
                    ?? throw new InvalidOperationException("Missing AdminAccount:Email");

        var logger = provider.GetRequiredService<ILogger<PostgresDbContext>>();

        var existingUser = context.Set<User>()
            .Select(u => new { u.Email, u.Role })
            .SingleOrDefault(u => u.Email == email);
        
        if (existingUser is not null)
        {
            if (existingUser.Role is not UserRole.Admin)
                throw new InvalidOperationException("AdminAccount:Email belongs to a non-admin account.");
            
            logger.LogWarning("This admin account existed.");
            return;
        }

        var password = adminAccount["Password"]
                        ?? throw new InvalidOperationException("Missing AdminAccount:Password");
        var fullName = adminAccount["FullName"]
                        ?? throw new InvalidOperationException("Missing AdminAccount:FullName");
        var phoneNumber = adminAccount["PhoneNumber"]
                            ?? throw new InvalidOperationException("Missing AdminAccount:PhoneNumber");

        var createdAdminResult = User.CreateAdmin(
            email: email,
            passwordHash: provider.GetRequiredService<IPasswordHashProvider>().Hash(password),
            fullName: fullName,
            phoneNumber: phoneNumber,
            createdAt: utcNow);
        
        if (createdAdminResult.IsFailure)
            throw new InvalidOperationException($"Admin seeding failed: {createdAdminResult.Error.Code}");
        
        context.Set<User>().Add(createdAdminResult.Value);

        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, IServiceProvider provider, IConfiguration configuration, CancellationToken cancellationToken)
    {
        if (!configuration.GetValue<bool>("AdminSeedingEnable"))
            return;
        
        var utcNow = provider.GetRequiredService<TimeProvider>().GetUtcNow().UtcDateTime;

        var adminAccount = configuration.GetRequiredSection("AdminAccount");

        var email = adminAccount["Email"]
                    ?? throw new InvalidOperationException("Missing AdminAccount:Email");

        var logger = provider.GetRequiredService<ILogger<PostgresDbContext>>();

        var existingUser = context.Set<User>()
            .Select(u => new { u.Email, u.Role })
            .SingleOrDefault(u => u.Email == email);
        
        if (existingUser is not null)
        {
            if (existingUser.Role is not UserRole.Admin)
                throw new InvalidOperationException("AdminAccount:Email belongs to a non-admin account.");
            
            logger.LogWarning("This admin account existed.");
            return;
        }

        var password = adminAccount["Password"]
                        ?? throw new InvalidOperationException("Missing AdminAccount:Password");
        var fullName = adminAccount["FullName"]
                        ?? throw new InvalidOperationException("Missing AdminAccount:FullName");
        var phoneNumber = adminAccount["PhoneNumber"]
                            ?? throw new InvalidOperationException("Missing AdminAccount:PhoneNumber");

        var createdAdminResult = User.CreateAdmin(
            email: email,
            passwordHash: provider.GetRequiredService<IPasswordHashProvider>().Hash(password),
            fullName: fullName,
            phoneNumber: phoneNumber,
            createdAt: utcNow);
        
        if (createdAdminResult.IsFailure)
            throw new InvalidOperationException($"Admin seeding failed: {createdAdminResult.Error.Code}");
        
        context.Set<User>().Add(createdAdminResult.Value);

        await context.SaveChangesAsync(cancellationToken);
    }
}