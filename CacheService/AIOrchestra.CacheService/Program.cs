using AIOrchestra.CacheService.Configuration;
using AIOrchestra.CacheService.Shared;
using KafkaLibrary;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);
ConfigureAppSettings(builder);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddKafka(builder.Configuration);
builder.Services.AddConsumers();
builder.Services.AddRedis(builder.Configuration);
SetMethodMappings(builder);

var app = builder.Build();
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
    var section = builder.Configuration.GetSection("MethodMappings");
    section.Bind(MethodMappingConfiguration.MethodMappings);
}