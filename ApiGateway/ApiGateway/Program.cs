using ApiGateway.Configuration;
using Carter;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddApplicationMediatR();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter();

var app = builder.Build();
app.MapCarter();
app.UseAuthentication();
app.UseAuthorization();

app.Run();  