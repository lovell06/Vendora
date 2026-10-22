var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Logging.AddSimpleConsole(options =>
{
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.SingleLine = true;
});

var host = builder.Build();
host.Run();
