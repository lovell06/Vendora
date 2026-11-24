using Microsoft.AspNetCore.OpenApi;
using Vendora.Services.Cart.Api.ExceptionHandlers;
using Vendora.Services.Cart.Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddOpenApi(options =>
{
    options.CreateSchemaReferenceId = info => info.Type.DeclaringType is { } parent
        ? parent.Name + info.Type.Name
        : OpenApiOptions.CreateDefaultSchemaReferenceId(info);
});

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers();

builder.Services.AddHttpCatalogServiceClient(builder.Configuration);

builder.Services
    .AddCurrentUserService()
    .AddPersistenceServices(builder.Configuration)
    .AddRepositoryServices()
    .AddQueryServices();

builder.Services
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddApplicationAuthorization();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ConflictExceptionHandler>();
builder.Services.AddExceptionHandler<UnhandledExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health").AllowAnonymous();

app.MapEndpoints();

app.Run();