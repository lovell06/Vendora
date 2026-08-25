namespace Vendora.BuildingBlocks.Results;

public class Error
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public ErrorType Type { get; init; }
}