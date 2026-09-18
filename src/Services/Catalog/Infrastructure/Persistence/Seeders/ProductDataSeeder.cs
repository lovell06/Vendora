using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Seeders;

public static class ProductDataSeeder
{
    private static readonly DateTime CreatedAt = new DateTimeOffset(
        new DateOnly(2026, 9, 12),
        new TimeOnly(12, 2, 11),
        TimeSpan.FromHours(7)).UtcDateTime;
    
    private static readonly Product[] Products =
    [
        Product.Create(
            "iPhone 15 128GB", 
            "A smartphone featuring a 6.1-inch OLED display and 128GB of storage.",
            "Apple",
            1,
            18_990_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "iPhone 15 Pro 256GB", 
            "A premium smartphone featuring a titanium design and 256GB of storage.",
            "Apple",
            1,
            27_990_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Samsung Galaxy S24 256GB", 
            "A premium Android smartphone with an AMOLED display and 256GB of storage.",
            "Samsung",
            1,
            19_990_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Xiaomi 14 256GB", 
            "A high-performance smartphone featuring an advanced camera and fast charging.",
            "Xiaomi",
            1,
            16_490_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "MacBook Air M3 13-inch", 
            "A lightweight laptop powered by the Apple M3 chip for studying and professional work.",
            "Apple",
            2,
            27_990_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "HP Pavilion 14", 
            "A compact laptop designed for students and office workers.",
            "HP",
            2,
            18_990_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Sony WH-1000XM5", 
            "Wireless over-ear headphones with active noise cancellation.",
            "Sony",
            3,
            7_490_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Apple AirPods Pro 2", 
            "True wireless earbuds featuring active noise cancellation and spatial audio.",
            "Apple",
            3,
            5_990_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Keychron K2 Pro", 
            "A wireless 75% mechanical keyboard supporting multiple operating systems.",
            "Keychron",
            4,
            2_590_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Logitech MX Keys Mini", 
            "A compact wireless keyboard designed for productivity and office work.",
            "Logitech",
            4,
            2_490_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Logitech MX Master 3S", 
            "A premium wireless mouse designed for productivity and creative work.",
            "Logitech",
            5,
            2_490_000m,
            "VND",
            true,
            CreatedAt).Value,
        Product.Create(
            "Razer DeathAdder V3", 
            "An ergonomic gaming mouse featuring a high-precision sensor.",
            "Razer",
            5,
            1_790_000m,
            "VND",
            true,
            CreatedAt).Value
    ];

    public static void Seed(DbContext context, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("InitializeProductEnable"))
            return;
        
        context.Set<Product>().AddRange(Products);
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, IConfiguration configuration, CancellationToken cancellationToken)
    {
        if (!configuration.GetValue<bool>("InitializeProductEnable"))
            return;
        
        context.Set<Product>().AddRange(Products);
        await context.SaveChangesAsync(cancellationToken);
    }
}