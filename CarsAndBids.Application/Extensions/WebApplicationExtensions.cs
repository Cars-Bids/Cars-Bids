using CarsAndBids.Data.Persistence.Seed;

namespace CarsAndBids.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            await seeder.SeedAsync();
        }
    }
}