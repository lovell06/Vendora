using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Identity.Api.Extensions;

public static class ErrorExtension
{
    public static IResult ToProblem(this Error error)
    {

        return error.Type switch
        {
            ErrorType.Conflict => Results.Conflict(error),
            ErrorType.Failure => Results.BadRequest(error),
            ErrorType.Forbidden => Results.Forbid(),
            ErrorType.NotFound => Results.NotFound(error),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Validation => Results.BadRequest(error),
            _ => throw new ArgumentOutOfRangeException(nameof(Error))
        };
    }
}