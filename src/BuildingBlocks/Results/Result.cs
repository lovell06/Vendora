namespace Vendora.BuildingBlocks.Results;

public class Result
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;

    public Error Error =>
        field ??
        throw new InvalidOperationException("Cannot access the error of a successful result.");
    
    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new Result(isSuccess: true, error: null);
    }

    public static Result Failure(Error error)
    {
        return new Result(isSuccess: false, error: error);
    }
}

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;

    public T Value =>
        IsSuccess ? field! :
        throw new InvalidOperationException("Cannot access the value of a failure result.");
    public Error Error =>
        IsFailure ? field! :
        throw new InvalidOperationException("Cannot access the error of a successful result.");
    
    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    private Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value: value);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(error: error);
    }
}