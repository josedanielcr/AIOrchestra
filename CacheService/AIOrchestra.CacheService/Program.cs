using AIOrchestra.CacheService.Configuration;
using KafkaLibrary;

var builder = WebApplication.CreateBuilder(args);
ConfigureAppSettings(builder);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddKafka(builder.Configuration);
builder.Services.AddConsumers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

static void ConfigureAppSettings(WebApplicationBuilder builder)
{
    var environment = Environment.GetEnvironmentVariable("RUNNING_IN_DOCKER");
    if (environment == "true")
    {
        builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: true, reloadOnChange: true);
    }
    else
    {
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    }
}