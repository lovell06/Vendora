namespace Vendora.Services.Identity.Infrastructure.Clients;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServiceClients(IConfiguration configuration)
        {
            services.AddCartServiceClient(configuration);

            return services;
        }

        public IServiceCollection AddCartServiceClient(IConfiguration configuration)
        {
            services.AddHttpClient<ICartClient, HttpCartClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:Cart:BaseUrl"]
                                             ?? throw new InvalidOperationException("Missing Services:Cart:BaseUrl"));

                client.Timeout = TimeSpan.FromSeconds(5);
            });
            return services;
        }
    }
}
