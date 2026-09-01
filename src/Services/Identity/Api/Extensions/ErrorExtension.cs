using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Identity.Api.Extensions;

public static class ErrorExtension
{
    public static IResult ToProblem(this Error error)
    {

        return error.Type switch
        {
            ErrorType.Conflict => TypedResults.Conflict(error),
            ErrorType.Failure => TypedResults.BadRequest(error),
            ErrorType.Forbidden => TypedResults.Forbid(),
            ErrorType.NotFound => TypedResults.NotFound(error),
            ErrorType.Unauthorized => TypedResults.Unauthorized(),
            ErrorType.Validation => TypedResults.BadRequest(error),
            _ => throw new ArgumentOutOfRangeException(nameof(Error))
        };
    }
}