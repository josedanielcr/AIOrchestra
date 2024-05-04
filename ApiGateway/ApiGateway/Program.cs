using ApiGateway.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();

app.Run();