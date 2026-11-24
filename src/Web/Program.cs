using Microsoft.Extensions.Options;
using MudBlazor.Services;
using Vendora.Web.Gateway;
using Vendora.Web.Components.Theming;
using Vendora.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddScoped<ThemeState>();

// Every backend call goes through the API Gateway.
builder.Services.Configure<GatewayOptions>(builder.Configuration.GetSection("ApiGateway"));
builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<CartState>();
builder.Services.AddHttpClient<GatewayClient>((provider, client) =>
{
    var options = provider.GetRequiredService<IOptions<GatewayOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl, UriKind.Absolute);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
