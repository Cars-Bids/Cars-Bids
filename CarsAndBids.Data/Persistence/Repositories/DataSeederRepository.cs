using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using CarsAndBids.Data.Persistence.NewFolder.Seed;
using CarsAndBids.Data.Persistence.SeedData;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Data.Persistence.Repositories;

public class DataSeederRepository(
    ApplicationDbContext context
    ) : IDataSeederRepository
{
    public async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
    {
        var roleSeed = new RoleSeed(roleManager);
        await roleSeed.SeedAsync();
    }

    public async Task SeedBodyStylesAsync(ApplicationDbContext context)
    {
        var bodyStyleRepository = new GenericRepository<BodyStyle>(context);
        var bodyStyleSeed = new BodyStyleSeed(bodyStyleRepository);
        await bodyStyleSeed.SeedAsync();
    }
}