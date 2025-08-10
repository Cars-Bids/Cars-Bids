using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Data.Persistence.Seed;

public class DataSeeder(
    RoleManager<IdentityRole<int>> roleManager,
    IGenericRepository<BodyStyle> bodyStyleRepository)
{
    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedBodyStylesAsync();
    }

    public async Task SeedRolesAsync()
    {
        var roleSeed = new RoleSeed(roleManager);
        await roleSeed.SeedAsync();
    }

    public async Task SeedBodyStylesAsync()
    {
        var bodyStyleSeed = new BodyStyleSeed(bodyStyleRepository);
        await bodyStyleSeed.SeedAsync();
    }
}