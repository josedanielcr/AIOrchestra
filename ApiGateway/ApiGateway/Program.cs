using ApiGateway.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.Run();