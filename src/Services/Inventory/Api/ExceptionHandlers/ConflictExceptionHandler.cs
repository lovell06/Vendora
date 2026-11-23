using Microsoft.AspNetCore.Diagnostics;
using Npgsql;
using Vendora.Services.Inventory.Application.Abstractions.Persistence.Exceptions;

namespace Vendora.Services.Inventory.Api.ExceptionHandlers;

public sealed class ConflictExceptionHandler(ILogger<ConflictExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var error = exception switch
        {
            ConcurrencyConflictException or DbUpdateConcurrencyException => Error.Conflict(
                code: "inventory.concurrency_conflict",
                message: "The resource was modified by another request. Please try again."),

            DbUpdateException
            {
                InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation }
            } => Error.Conflict(
                code: "inventory.duplicate",
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