using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Application.Features.CreateCategory;

public class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateCategoryCommandHandler> logger,
    TimeProvider clock) : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        var createdCategoryResult = Category.Create(command.Name, utcNow);

        if (createdCategoryResult.IsFailure)
        {
            logger.LogWarning("Category creation failed: {ErrorCode}", createdCategoryResult.Error.Code);

            return Result.Failure(createdCategoryResult.Error);
        }

        categoryRepository.Add(createdCategoryResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}