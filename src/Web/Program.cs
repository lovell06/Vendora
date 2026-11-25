using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Vendora.Web.Localization;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Vendora.Web.Authentication;
using Vendora.Web.Gateway;
using Vendora.Web.Gateway.Admin;
using Vendora.Web.Components.Theming;
using Vendora.Web.Components;
using Vendora.Web.Components.UI.Layout;
using Vendora.Web.Features.Cart;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// UI language: English by default, Vietnamese optional. The choice is kept in the culture cookie and
// picked up by the circuit when it starts, so switching reloads the page.
builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(Cultures.Default)
        .AddSupportedCultures(Cultures.Supported)
        .AddSupportedUICultures(Cultures.Supported);
    options.RequestCultureProviders = [new CookieRequestCultureProvider()];
});
builder.Services.AddScoped<ThemeState>();

// Role checks for /admin: the principal is built from the signed-in UserSession.
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, SessionAuthenticationStateProvider>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationMiddlewareResultHandler, PassThroughAuthorizationResultHandler>();
builder.Services.Configure<StoreInfoOptions>(builder.Configuration.GetSection("Store"));

// Every backend call goes through the API Gateway.
builder.Services.Configure<GatewayOptions>(builder.Configuration.GetSection("ApiGateway"));
builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<CartState>();
builder.Services.AddScoped<ViewportState>();
builder.Services.AddScoped<CartUiState>();
builder.Services.AddScoped<GatewayClient>();
builder.Services.AddScoped<SignOutService>();
builder.Services.AddScoped<Vendora.Web.Features.Admin.Products.Create.ProductDraft>();
builder.Services.AddScoped<AdminGatewayClient>();
builder.Services.AddHttpClient<GatewayTransport>((provider, client) =>
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

app.UseRequestLocalization();
app.UseAntiforgery();

app.MapGet("/culture/set", (string culture, string? redirectUri, HttpContext context) =>
{
    if (Cultures.Supported.Contains(culture))
    {
        context.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true, SameSite = SameSiteMode.Lax });
    }
    var target = !string.IsNullOrEmpty(redirectUri) && redirectUri[0] == '/' && (redirectUri.Length == 1 || (redirectUri[1] != '/' && redirectUri[1] != '\\'))
        ? redirectUri : "/";
    return Results.LocalRedirect(target);
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
