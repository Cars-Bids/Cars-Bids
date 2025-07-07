using CarsAndBids.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarsAndBids.Data.Persistence
{
    public class DataSeeder(
        ApplicationDbContext context, 
        RoleManager<IdentityRole<int>> roleManager, 
        IDataSeederRepository seederRepository
        )
    {
        public async Task SeedAsync()
        {
            await seederRepository.SeedRolesAsync(roleManager);
            await seederRepository.SeedBodyStylesAsync(context);

        }

    }

}