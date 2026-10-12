namespace Vendora.BuildingBlocks.Results;

public class Error
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public ErrorType Type { get; init; }

    public override string ToString()
    {
        return $"{Type}: {Code}. {Message}";
    }

    public ErrorResponse ToResponse()
    {
        return new ErrorResponse { Code = Code, Message = Message, Type = Type.ToString() };
    }

    public static Error FromResponse(ErrorResponse errorResponse)
    {
        ArgumentNullException.ThrowIfNull(errorResponse);

        if (!(Enum.TryParse<ErrorType>(errorResponse.Type, out var errorType)
            && Enum.IsDefined(errorType)))
        {
            throw new InvalidOperationException($"Unsupported error type: {errorResponse.Type}");
        }

        return new Error
        {
            Code = errorResponse.Code,
            Message = errorResponse.Message,
            Type = errorType
        };
    }
}