using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Data.Persistence.Seed;

public class RoleSeed(RoleManager<IdentityRole<int>> roleManager)
{
    public async Task SeedAsync()
    {
        string[] roles = { "Admin", "Manager", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
            }
        }
    }
}
