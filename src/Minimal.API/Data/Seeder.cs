using Microsoft.EntityFrameworkCore;

namespace Minimal.API.Data;

/// <summary>Provides database seeding functionality for the application.</summary>
public static class Seeder
{
    /// <summary>
    /// Recreates and migrates the database on each application run. This method is primarily used for API testing purposes.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance representing the application.</param>
    public static void RecreateDatabase(WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();
        
        var context = serviceScope.ServiceProvider.GetRequiredService<DishesDbContext>();
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }
}