namespace Vendora.BuildingBlocks.Results;

public sealed class ErrorResponse
{
    public required string Code { get; init; }
    public required string Message { get; init; }
    public required string Type { get; init; }
}