using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Domain.Categories;

public class Category
{
    public int Id { get; init; }
    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public ICollection<Product> Products { get; init; } = [];

    public bool IsDeleted => DeletedAt is not null;

    private Category()
    {
    }

    public static Result<Category> Create(string name, DateTime createdAt)
    {
        name = name.Trim();

        if (string.IsNullOrEmpty(name))
        {
            return Result<Category>.Failure(new Error
            {
                Code = "category_create.name_is_null",
                Message = "Category name is required.",
                Type = ErrorType.Validation
            });
        }
        
        return Result<Category>.Success(new Category
        {
            Id = 0,
            Name = name,
            CreatedAt = createdAt,
            UpdatedAt = null,
            DeletedAt = null
        });
    }

    public Result Rename(string name, DateTime updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "category_rename.deleted",
                Message = "Cannot rename a deleted category.",
                Type = ErrorType.Validation
            });
        }

        Name = name;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public void Delete(DateTime deletedAt)
    {
        if (IsDeleted)
            return;
        
        DeletedAt = deletedAt;
        UpdatedAt = deletedAt;
    }

    public void Restore(DateTime updatedAt)
    {
        if (IsDeleted)
            return;
        
        DeletedAt = null;
        UpdatedAt = updatedAt;
    }
}