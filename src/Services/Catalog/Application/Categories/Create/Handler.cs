using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Application.Categories.Create
{
    public class Handler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<Handler> logger,
        TimeProvider clock) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
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
}