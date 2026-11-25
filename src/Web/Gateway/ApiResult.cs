using System.Net;

namespace Vendora.Web.Gateway;

public readonly record struct NoContent;

public record ApiResult(bool Ok, string? Error, HttpStatusCode Status)
{
    public string? Code { get; init; }
    public bool NotFound => Status == HttpStatusCode.NotFound;
    public bool Unauthorized => Status == HttpStatusCode.Unauthorized;
    public bool Forbidden => Status == HttpStatusCode.Forbidden;
    public bool Conflict => Status == HttpStatusCode.Conflict;
    public bool PayloadTooLarge => Status == HttpStatusCode.RequestEntityTooLarge;
}

public sealed record ApiResult<T>(bool Ok, T? Value, string? Error, HttpStatusCode Status)
    : ApiResult(Ok, Error, Status)
{
    public static ApiResult<T> Success(T value, HttpStatusCode status) => new(true, value, null, status);
    public static ApiResult<T> Fail(string error, HttpStatusCode status, string? code = null) =>
        new(false, default, error, status) { Code = code };
}
