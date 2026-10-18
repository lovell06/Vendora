namespace Vendora.Services.Inventory.Api.Extensions;

public static class ErrorExtension
{
    public static IResult ToHttpResult(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => TypedResults.BadRequest(error.ToResponse()),

            ErrorType.Conflict => TypedResults.Conflict(error.ToResponse()),

            ErrorType.NotFound => TypedResults.NotFound(error.ToResponse()),

            ErrorType.Failure => TypedResults.InternalServerError(error.ToResponse()),

            ErrorType.Unauthorized => TypedResults.Unauthorized(),

            ErrorType.Forbidden => TypedResults.Forbid(),
            
            _ => throw new InvalidOperationException(nameof(error.Type))
        };
    }
}