using Microsoft.EntityFrameworkCore;

namespace Vendora.Services.Identity.Infrastructure.Persistence;

public class PostgresDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);
    }
}