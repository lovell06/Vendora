using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Seeders;

public static class CategoryDataSeeder
{
    public static void Seed(DbContext context)
    {
        context.Set<Category>().AddRange(Categories);
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        context.Set<Category>().AddRange(Categories);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static readonly DateTime CreatedAt = new DateTimeOffset(
        new DateOnly(2026, 9, 11),
        new TimeOnly(11, 11, 11),
        TimeSpan.FromHours(7)).UtcDateTime;

    private static readonly Category[] Categories =
    [
        Category.Create("Smartphones", CreatedAt).Value,
        Category.Create("Laptops", CreatedAt).Value,
        Category.Create("Headphones", CreatedAt).Value,
        Category.Create("Keyboards", CreatedAt).Value,
        Category.Create("Mice", CreatedAt).Value
    ];
}