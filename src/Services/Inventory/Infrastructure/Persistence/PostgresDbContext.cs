using Microsoft.EntityFrameworkCore;
using Vendora.Services.Inventory.Domain.InventoryItems;

namespace Vendora.Services.Inventory.Infrastructure.Persistence;

public sealed class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);
    }
}