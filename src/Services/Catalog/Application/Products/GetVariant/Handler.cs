namespace Vendora.Services.Catalog.Application.Products.GetVariant;

public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await queryService.ExecuteAsync(query, cancellationToken);

        if (response is null)
            return Result<Response>.Failure(new Error
            {
                Code = "getting_product_variant.not_found",
                Message = "Product variant not found",
                Type = ErrorType.NotFound
            });
        
        return Result<Response>.Success(response);
    }
}