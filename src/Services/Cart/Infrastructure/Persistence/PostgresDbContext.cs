using Microsoft.EntityFrameworkCore;
using CartAggregate = Vendora.Services.Cart.Domain.Carts.Cart;

namespace Vendora.Services.Cart.Infrastructure.Persistence;

public sealed class PostgresDbContext(
    DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    public DbSet<CartAggregate> Carts => Set<CartAggregate>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);
    }
}