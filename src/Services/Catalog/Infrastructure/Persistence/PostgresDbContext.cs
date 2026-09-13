using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Domain.Categories;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Infrastructure.Persistence;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PostgresDbContext).Assembly);
    }
}