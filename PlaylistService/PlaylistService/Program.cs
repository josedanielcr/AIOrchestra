using CacheLibrary;
using KafkaLibrary;
using PlaylistService.Configurations;
using PlaylistService.Shared;

var builder = WebApplication.CreateBuilder(args);
ConfigureAppSettings(builder);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddKafka(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddConsumers();

// internal services
builder.Services.AddScoped<PlaylistDbUtils>();

// methods
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