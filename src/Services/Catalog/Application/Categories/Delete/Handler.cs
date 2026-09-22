using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Application.Categories.Delete;

public sealed class Handler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var category = await categoryRepository.GetByIdAsync(cmd.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Deleting category rejected because category does not exist.");

            return Result.Failure(new Error
            {
                Code = "category_delete.failed",
                Message = "Category is not found.",
                Type = ErrorType.NotFound
            });
        }

        category.Delete(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}