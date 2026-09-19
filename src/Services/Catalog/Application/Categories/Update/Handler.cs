using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Application.Categories.Update;

public sealed class Handler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        var category = await categoryRepository.GetByIdAsync(cmd.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Cateogory update rejected because category does not exist.");

            return Result.Failure(new Error
            {
                Code = "category_update.failed",
                Message = "Category is not found.",
                Type = ErrorType.NotFound
            });
        }

        category.Rename(cmd.Name, utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}