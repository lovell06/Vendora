using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Categories;
using Vendora.Services.Catalog.Domain.Products;
using Vendora.Services.Catalog.Infrastructure.Persistence.Repositories;
using Vendora.Services.Catalog.Infrastructure.Persistence.Seeders;
using Vendora.Services.Catalog.Infrastructure.Persistence.UnitOfWork;

namespace Vendora.Services.Catalog.Infrastructure.Persistence;

internal static class DependencyInjection
{
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostgresDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));

            options.UseSeeding((context, _) =>
            {
                CategoryDataSeeder.Seed(context);
                ProductDataSeeder.Seed(context);
            });

            options.UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                await CategoryDataSeeder.SeedAsync(context, cancellationToken);
                await ProductDataSeeder.SeedAsync(context, cancellationToken);
            });
        });

        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();
        services.AddScoped<IProductRepository, PostgresProductRepository>();
        services.AddScoped<ICategoryRepository, PostgresCategoryRepository>();
        return services;
    }
}