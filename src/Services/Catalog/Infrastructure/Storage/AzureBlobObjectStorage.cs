using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Vendora.Services.Catalog.Application.Abstractions.Storage;

namespace Vendora.Services.Catalog.Infrastructure.Storage;

public sealed class AzureBlobObjectStorage(BlobContainerClient container) : IObjectStorage
{
    public async Task<string> StoreAsync(Stream content, string contentType, string prefix, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(prefix);

        await container.CreateIfNotExistsAsync(cancellationToken: ct);

        var normalizedPrefix = prefix.Trim('/');
        var name = Guid.CreateVersion7().ToString("N");

        var objectKey = name.Length == 0
            ? name
            : $"{normalizedPrefix}/{name}";

        var blob = container.GetBlobClient(objectKey);

        await blob.UploadAsync(
            content: content,
            options: new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } },
            cancellationToken: ct);

        return objectKey;
    }

    public async Task<Stream?> ReadAsStreamAsync(string objectKey, CancellationToken ct = default)
    {
        await container.CreateIfNotExistsAsync(cancellationToken: ct);

        var blob = container.GetBlobClient(objectKey);

        if (!(await blob.ExistsAsync(ct)).Value)
            return null;

        return await blob.OpenReadAsync(cancellationToken: ct);
    }

    public async Task DeleteIfExistsAsync(string objectKey, CancellationToken ct = default)
    {
        await container.CreateIfNotExistsAsync(cancellationToken: ct);

        await container
            .GetBlobClient(objectKey)
            .DeleteIfExistsAsync(cancellationToken: ct);
    }
}