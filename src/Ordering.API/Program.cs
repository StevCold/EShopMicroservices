using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

app.UseApiServices();

// Initialize database
if (app.Environment.IsDevelopment())
{
    try
    {
        await app.InitialDatabaseAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database init failed: {ex.Message}");
    }
}

app.Run();
