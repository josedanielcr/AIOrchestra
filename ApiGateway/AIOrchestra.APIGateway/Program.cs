using AIOrchestra.APIGateway.Configurations.Authentication;
using AIOrchestra.APIGateway.Configurations.Kafka;
using AIOrchestra.APIGateway.Configurations.Packages;
using Carter;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
ConfigureAppSettings(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddKafkaProducer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();
app.UseAuthentication();
app.UseAuthorization();
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