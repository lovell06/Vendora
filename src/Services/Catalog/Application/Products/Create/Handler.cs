namespace Vendora.Services.Catalog.Application.Products.Create;

public sealed class Handler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IOutboxRepository outboxRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

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

        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        productRepository.Add(product);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            outboxRepository.Add(new ProductCreatedEvent
            {
                Id = Guid.CreateVersion7(),
                OccurredAt = utcNow,
                ProductId = product.Id
            }, utcNow);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}