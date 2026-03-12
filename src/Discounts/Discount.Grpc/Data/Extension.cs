using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public static class Extension
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        context.Database.Migrate(); // Changed from MigrateAsync to Migrate (synchronous)

        return app;
    }
}

