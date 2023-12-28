using Microsoft.EntityFrameworkCore;

namespace LexiconLMS.Server.Data;

public static class DbInitializerExtension
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var serviceProvider = scope.ServiceProvider;
        var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Delete the database if it exists
        await db.Database.EnsureDeletedAsync();

        // Run all the migrations, if the database doesn't exist create it, if it exist, just update the database
        await db.Database.MigrateAsync();

        try
        {
            await DbInitializer.InitAsync(db, serviceProvider);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}