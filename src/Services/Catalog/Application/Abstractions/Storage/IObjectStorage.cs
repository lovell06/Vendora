namespace Vendora.Services.Catalog.Application.Abstractions.Storage;

public interface IObjectStorage
{
    Task<string> StoreAsync(
        Stream content, 
        string contentType,
        string prefix,
        CancellationToken ct = default);

    Task<Stream?> ReadAsStreamAsync(
        string objectKey,
        CancellationToken ct = default);

    Task DeleteIfExistsAsync(
        string objectKey,
        CancellationToken ct = default);
}