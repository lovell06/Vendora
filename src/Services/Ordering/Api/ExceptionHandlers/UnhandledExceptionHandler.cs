using Microsoft.AspNetCore.Diagnostics;

namespace Vendora.Services.Ordering.Api.ExceptionHandlers;

public sealed class UnhandledExceptionHandler(ILogger<UnhandledExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            exception: exception,
            message: "Unhandled on {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(Error.Failure(
            code: "server.unexpected_error",
            message: "An unexpected error occurred."), cancellationToken);

        return true;
    }
}