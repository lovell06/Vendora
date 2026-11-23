using Microsoft.AspNetCore.Diagnostics;
using Npgsql;

namespace Vendora.Services.Identity.Api.ExceptionHandlers;

public sealed class ConflictExceptionHandler(ILogger<ConflictExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var error = exception switch
        {
            DbUpdateConcurrencyException => Error.Conflict(
                code: "identity.concurrency_conflict",
                message: "The identity was modified by another request. Please retry again."),

            DbUpdateException
            {
                InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation }
            } => Error.Conflict(
                code: "identity.conflict",
                message: "A record with the same unique value already exists."),

            _ => null
        };

        if (error is null)
            return false;
        
        logger.LogWarning(
            "{Method} {Path} returned 409 ({Code}).", 
            httpContext.Request.Method, httpContext.Request.Path, error.Code);

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(error.ToResponse(), cancellationToken);

        return true;
    }
}