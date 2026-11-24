using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Vendora.Services.Catalog.Api.ExceptionHandlers;
using Vendora.Services.Catalog.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Servers = [new OpenApiServer { Url = "/" }];
        return Task.CompletedTask;
    });

    options.CreateSchemaReferenceId = info => info.Type.DeclaringType is { } parent
        ? parent.Name + info.Type.Name
        : OpenApiOptions.CreateDefaultSchemaReferenceId(info);
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers();

builder.Services
    .AddCurrentUserService()
    .AddServiceClients(builder.Configuration)
    .AddPersistenceServices(builder.Configuration)
    .AddQueryServices()
    .AddStorageServices(builder.Configuration)
    .AddRepositoryServices()
    .AddEventTypeRegistry();

builder.Services
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddApplicationAuthorization();

builder.Logging.AddSimpleConsole(config =>
{
    config.SingleLine = true;
    config.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    config.UseUtcTimestamp = true;
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ConflictExceptionHandler>();
builder.Services.AddExceptionHandler<UnhandledExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await using var scope = app.Services.CreateAsyncScope();

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await db.Database.MigrateAsync();
}

using (var scope = app.Services.CreateScope())
{
    var container = scope.ServiceProvider.GetRequiredService<BlobContainerClient>();

    await container.CreateIfNotExistsAsync();
    await container.SetAccessPolicyAsync(PublicAccessType.Blob);
}

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/health").AllowAnonymous();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();