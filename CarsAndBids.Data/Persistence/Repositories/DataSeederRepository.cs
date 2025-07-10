using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Persistence.Seed;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Data.Persistence.Repositories;

public class DataSeederRepository(
    ApplicationDbContext context,
    IGenericRepository<BodyStyle> bodyStyleRepository,
    RoleManager<IdentityRole<int>> roleManager
    ) : IDataSeederRepository
{
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