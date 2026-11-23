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

    public static Error Unexpected(string code, string message) => new()
    {
        Code = code,
        Message = message,
        Type = ErrorType.Unexpected
    };

    public static Error Validation(string code, string message) => new()
    {
        Code = code,
        Message = message,
        Type = ErrorType.Validation
    };

    public static Error NotFound(string code, string message) => new()
    {
        Code = code,
        Message = message,
        Type = ErrorType.NotFound
    };

    public static Error Conflict(string code, string message) => new()
    {
        Code = code,
        Message = message,
        Type = ErrorType.Conflict
    };

    public static Error Unauthorized() => new()
    {
        Code = string.Empty,
        Message = string.Empty,
        Type = ErrorType.Unauthorized
    };

    public static Error Forbidden() => new()
    {
        Code = string.Empty,
        Message = string.Empty,
        Type = ErrorType.Forbidden
    };
}