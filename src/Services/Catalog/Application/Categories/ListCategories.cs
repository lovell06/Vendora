namespace Vendora.Services.Catalog.Application.Categories;

public static class ListCategories
{
    public sealed record Query(int Page, int Size) : IQuery<Response>;

    public sealed record Response(int Page, int TotalCount, IReadOnlyList<CategoryDto> Categories);

    public sealed record CategoryDto(int Id, string Name);

    public interface IQueryService
    {
        Task<Response> ExecuteAsync(
            Query query,
            CancellationToken cancellationToken);
    }

    public sealed class Handler(IQueryService listCategoryQueryService) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await listCategoryQueryService.ExecuteAsync(query, cancellationToken);
            return Result<Response>.Success(response);
        }
    }
}
