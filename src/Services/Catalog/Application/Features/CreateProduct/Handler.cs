using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Categories;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Application.Features.CreateProduct;

public class Handler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        if (!await categoryRepository.ExistsByIdAsync(command.CategoryId, cancellationToken))
        {
            logger.LogWarning("Product creation rejected because category not existed.");

            return Result.Failure(new Error
            {
                Code = "product_creation.category_is_not_exists",
                Message = "Category is not exists.",
                Type = ErrorType.Validation
            });
        }

        var createdProductResult = Product.Create(
            name: command.Name,
            description: command.Description,
            brand: command.Brand,
            categoryId: command.CategoryId,
            price: command.Price,
            currency: command.Currency,
            isVisible: command.IsVisible,
            createdAt: utcNow);
        
        if (createdProductResult.IsFailure)
        {
            logger.LogWarning(
                "Create product failed: {ErrorCode}", 
                createdProductResult.Error.Code);
            
            return Result.Failure(createdProductResult.Error);
        }

        var product = createdProductResult.Value;

        productRepository.Add(product);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}