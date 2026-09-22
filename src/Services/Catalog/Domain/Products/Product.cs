using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Domain.Products;

public class Product
{
    public long Id { get; init; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Brand { get; private set; } = null!;
    public int CategoryId { get; private set; }
    public Category? Category { get; private set; } = null;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = null!;
    public ProductStatus Status { get; private set; }
    public bool IsVisible { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt is not null;

    private Product()
    {
    }

    public static Result<Product> Create(
        string name,
        string? description,
        string brand,
        int categoryId,
        decimal price,
        string currency,
        bool isVisible,
        DateTimeOffset createdAt)
    {
        name = name.Trim();
        description = description?.Trim();
        brand = brand.Trim();

        if (string.IsNullOrEmpty(name))
        {
            return Result<Product>.Failure(new Error
            {
                Code = "product_create.name_is_null",
                Message = "Product name is requered.",
                Type = ErrorType.Validation
            });
        }

        if (string.IsNullOrEmpty(brand))
        {
            return Result<Product>.Failure(new Error
            {
                Code = "product_create.brand_is_null",
                Message = "Product brand is requered.",
                Type = ErrorType.Validation
            });
        }

        if (price < 0)
        {
            return Result<Product>.Failure(new Error
            {
                Code = "product_create.price_is_negative",
                Message = "Product price must be more than or equal zero.",
                Type = ErrorType.Validation
            });
        }

        if (categoryId < 0)
        {
            return Result<Product>.Failure(new Error
            {
                Code = "product_create.category_invalid",
                Message = "Category ID must not be negative.",
                Type = ErrorType.Validation
            });
        }

        return Result<Product>.Success(new Product
        {
            Id = 0,
            Name = name,
            Description = description,
            Brand = brand,
            CategoryId = categoryId,
            Price = price,
            Currency = currency,
            Status = ProductStatus.Active,
            IsVisible = isVisible,
            CreatedAt = createdAt,
            UpdatedAt = null,
            DeletedAt = null
        });
    }

    public Result Rename(string name, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_rename.deleted",
                Message = "Cannot rename a deleted product.",
                Type = ErrorType.Validation
            });
        }
        
        Name = name;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangeDescription(string description, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_change_description.deleted",
                Message = "Cannot change description of a deleted product.",
                Type = ErrorType.Validation
            });
        }

        Description = description;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangeBrand(string brandName, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_change_brand.deleted",
                Message = "Cannot change brand of a deleted product.",
                Type = ErrorType.Validation
            });
        }

        Brand = brandName;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangeCategory(int categoryId, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_change_category.deleted",
                Message = "Cannot change category of a deleted product.",
                Type = ErrorType.Validation
            });
        }

        CategoryId = categoryId;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangePrice(decimal price, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_change_price.deleted",
                Message = "Cannot change price of a deleted product.",
                Type = ErrorType.Validation
            });
        }

        Price = price;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result Discontinue(DateTimeOffset discontinuedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_discontinue.deleted",
                Message = "Cannot discontinue a deleted product.",
                Type = ErrorType.Validation
            });
        }

        if (Status is ProductStatus.Discontinued)
            return Result.Success();

        Status = ProductStatus.Discontinued;
        UpdatedAt = discontinuedAt;

        return Result.Success();
    }

    public Result Reactivate(DateTimeOffset reactivedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_continue.deleted",
                Message = "Cannot reactivate a deleted product.",
                Type = ErrorType.Validation
            });
        }

        if (Status is ProductStatus.Active)
            return Result.Success();

        Status = ProductStatus.Active;
        UpdatedAt = reactivedAt;

        return Result.Success();
    }

    public void Hide(DateTimeOffset updatedAt)
    {
        if (!IsVisible)
            return;
        
        IsVisible = false;
        UpdatedAt = updatedAt;
    }

    public void Show(DateTimeOffset updatedAt)
    {
        if (IsVisible)
            return;

        IsVisible = true;
        UpdatedAt = updatedAt;
    }

    public void Delete(DateTimeOffset deletedAt)
    {
        if (IsDeleted)
            return;

        DeletedAt = deletedAt;
        UpdatedAt = deletedAt;
    }

    public void Restore(DateTimeOffset restoredAt)
    {
        if (!IsDeleted)
            return;

        DeletedAt = null;
        UpdatedAt = restoredAt;
    }
}