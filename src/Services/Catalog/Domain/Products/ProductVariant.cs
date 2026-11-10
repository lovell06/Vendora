using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Domain.Products;

public class ProductVariant
{
    public long Id { get; init; }
    public required string Sku { get; init; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public long ProductId { get; init; }
    public Product? Product { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt is not null;

    private ProductVariant()
    {
    }

    public static Result<ProductVariant> Create(
        string sku,
        string name,
        decimal price,
        long productId,
        DateTimeOffset createdAt)
    {
        sku = sku.Trim();
        if (string.IsNullOrEmpty(sku))
            return Result<ProductVariant>.Failure(Errors.SkuIsNullOrEmpty);
        
        if (string.IsNullOrWhiteSpace(name))
            return Result<ProductVariant>.Failure(Errors.ProductVariantNameIsNullOrEmpty);
        
        if (price < 0)
            return Result<ProductVariant>.Failure(Errors.ProductVariantPriceIsNegative);
        
        if (productId < 0)
            return Result<ProductVariant>.Failure(Errors.InvalidProductId);
        
        return Result<ProductVariant>.Success(new ProductVariant
        {
            Id = 0,
            Sku = sku,
            Name = name,
            Price = price,
            ProductId = productId,
            CreatedAt = createdAt
        });
    }

    public Result ChangeName(string name, DateTimeOffset updatedAt)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(Errors.ProductVariantNameIsNullOrEmpty);

        Name = name;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangePrice(decimal price, DateTimeOffset updatedAt)
    {
        if (price < 0)
            return Result.Failure(Errors.ProductVariantPriceIsNegative);

        Price = price;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result Delete(DateTimeOffset deletedAt)
    {
        if (IsDeleted)
            return Result.Success();

        DeletedAt = deletedAt;
        UpdatedAt = deletedAt;

        return Result.Success();
    }
}