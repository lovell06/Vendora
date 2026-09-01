using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Identity.Api.Extensions;

public static class ErrorExtension
{
    public static IResult ToHttpResult(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Conflict => TypedResults.Conflict(new
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
            
            ErrorType.Forbidden => TypedResults.Forbid(),

            ErrorType.NotFound => TypedResults.NotFound(new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            }),
            
            ErrorType.Unauthorized => TypedResults.Unauthorized(),

            ErrorType.Validation => TypedResults.BadRequest(new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            }),
            
            _ => throw new ArgumentOutOfRangeException(nameof(Error.Type))
        };
    }
}