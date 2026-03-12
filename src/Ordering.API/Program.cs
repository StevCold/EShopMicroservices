using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//add services to the container

//configure the http request pipeline
// ----------------------------------
// Infrastrucrue - EF Core
// Application - MediatR
// API - Controllers

//builder.Services.
//     .AddApplicationServices()
//     .AddInfrastructureServices(builder.Configuration)
//     .AddApiServices();
// ----------------------------------

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();


var app = builder.Build();
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialDatabaseAsync();
}

//configure the http request pipeline
app.Run();
