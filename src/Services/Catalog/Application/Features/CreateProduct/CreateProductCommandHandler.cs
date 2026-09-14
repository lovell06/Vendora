using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Categories;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Application.Features.CreateProduct;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateProductCommandHandler> logger,
    TimeProvider clock) : ICommandHandler<CreateProductCommand>
{
    public async Task<Result> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        if (!await categoryRepository.ExistsByIdAsync(command.CategoryId, cancellationToken))
        {
            logger.LogWarning("Product creation rejected because category not existed.");

            return Result.Failure(new Error
            {
                Code = "product_creation.category_not_found",
                Message = "Category not found.",
                Type = ErrorType.NotFound
            });
        }

        var createdProductResult = Product.Create(
            name: command.Name,
            description: command.Description,
            brand: command.Brand,
            categoryId: command.CategoryId,
            price: command.Price,
            isVisible: command.IsVisible,
            createdAt: utcNow);
        
        if (createdProductResult.IsFailure)
        {
            logger.LogWarning(
                "Create product rejected because {err}", 
                createdProductResult.Error.Code);
            
            return Result.Failure(createdProductResult.Error);
        }

        var product = createdProductResult.Value;

        productRepository.Add(product);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}