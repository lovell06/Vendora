namespace Vendora.Web.Features.Admin.Products.Shared;

public sealed class ImageItem
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Data { get; init; }
    public string? Preview { get; init; }
    public long Size => Data.LongLength;
}
