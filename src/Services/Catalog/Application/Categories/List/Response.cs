namespace Vendora.Services.Catalog.Application.Categories.List;

public sealed class Response
{
    public int Page { get; init; }
    public int TotalCount { get; init; }
    public required IReadOnlyList<CategoryDto> Categories { get; init; }
}

public sealed class CategoryDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
}