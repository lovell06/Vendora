using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Cart.Api.Extensions;

public static class ErrorExtension
{
    public static IResult ToHttpResult(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => TypedResults.BadRequest(new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            }),

            ErrorType.Conflict => TypedResults.Conflict(new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            }),

            ErrorType.NotFound => TypedResults.NotFound(new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            }),

            ErrorType.Failure => TypedResults.InternalServerError(new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            }),

            ErrorType.Unauthorized => TypedResults.Unauthorized(),

            ErrorType.Forbidden => TypedResults.Forbid(),
            
            _ => throw new InvalidOperationException(nameof(error.Type))
        };
    }
}