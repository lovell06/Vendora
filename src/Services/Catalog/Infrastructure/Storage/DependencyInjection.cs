using Azure.Storage.Blobs;
using Vendora.Services.Catalog.Application.Abstractions.Storage;

namespace Vendora.Services.Catalog.Infrastructure.Storage;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddStorageServices(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureBlobStorage")
                                   ?? throw new InvalidOperationException(
                                       "Missing ConnectionStrings:AzureBlobStorage configuration.");

            var containerName = configuration["AzureBlobStorage:ContainerName"]
                                ?? throw new InvalidOperationException(
                                    "Missing AzureBlobStorage:ContainerName configuration.");

            services.AddSingleton<BlobContainerClient>(_ =>
                new BlobContainerClient(
                    connectionString: connectionString,
                    blobContainerName: containerName));
            
            services.AddSingleton<IObjectStorage, AzureBlobObjectStorage>();
            return services;
        }
    }
}