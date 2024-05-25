using AIOrchestra.CacheService.Configuration;
using KafkaLibrary;
using Microsoft.Extensions.Options;
using SharedLibrary;
using SharedLibrary.Configuration;

var builder = WebApplication.CreateBuilder(args);
ConfigureAppSettings(builder);
SetMethodMappings(builder);
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

static void SetMethodMappings(WebApplicationBuilder builder)
{
    var methodMappingConfiguration = new MethodMappingConfiguration();
    var section = builder.Configuration.GetSection("MethodMappings");
    methodMappingConfiguration.MethodMappings = new Dictionary<string, string>();

    foreach (var child in section.GetChildren())
    {
        methodMappingConfiguration.MethodMappings[child.Key] = child.Value;
    }
    InvokeMethodHelper.Initialize(methodMappingConfiguration.MethodMappings);
}