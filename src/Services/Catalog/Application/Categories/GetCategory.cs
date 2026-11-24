namespace Vendora.Services.Catalog.Application.Categories;

public static class GetCategory
{
    public sealed record Query(int Id) : IQuery<Response>;

    public sealed record Response(int Id, string Name);

    public interface IQueryService
    {
        Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
    }

    public sealed class Handler(
        IQueryService getCategoryQueryService,
        ILogger<Handler> logger) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await getCategoryQueryService.ExecuteAsync(query, cancellationToken);

            if (response is null)
            {
                logger.LogWarning("Category retrieval rejected because category not found.");

                return Result<Response>.Failure(new Error
                {
                    Code = "category_retrieval.not_found",
                    Message = "Category is not found.",
                    Type = ErrorType.NotFound
                });
            }

            return Result<Response>.Success(response);
        }
    }
}
