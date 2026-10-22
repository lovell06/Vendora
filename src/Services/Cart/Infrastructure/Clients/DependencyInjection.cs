namespace Vendora.Services.Cart.Infrastructure.Clients;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddHttpCatalogServiceClient(IConfiguration configuration)
        {
            services.AddHttpClient<ICatalogServiceClient, HttpCatalogServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:Catalog:BaseAddress"]
                                             ?? throw new InvalidOperationException(
                                                 "Missing Services:Catalog:BaseAddress configuration."));

                client.Timeout = TimeSpan.FromSeconds(5);
            });

            return services;
        }
    }
}