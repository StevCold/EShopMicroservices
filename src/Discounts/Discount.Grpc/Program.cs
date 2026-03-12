using Discount.Grpc.Services;
using Discount.Grpc.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<DiscountContext>(opts =>
    opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddGrpcReflection();

var app = builder.Build();

// Add request logging middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("=== Incoming gRPC Request ===");
    logger.LogInformation("Path: {Path}", context.Request.Path);
    logger.LogInformation("Method: {Method}", context.Request.Method);
    logger.LogInformation("ContentType: {ContentType}", context.Request.ContentType);

    await next();

    logger.LogInformation("Response StatusCode: {StatusCode}", context.Response.StatusCode);
});

// Configure the HTTP request pipeline.
app.UseMigration();

app.MapGrpcService<DiscountService>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();