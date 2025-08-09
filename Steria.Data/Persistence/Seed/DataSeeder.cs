using Steria.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarsAndBids.Data.Persistence.Seed
{
    public class DataSeeder(
        IDataSeederRepository seederRepository
        )
    {
        public async Task SeedAsync()
        {
            await seederRepository.SeedRolesAsync();
            await seederRepository.SeedBodyStylesAsync();

        }

    }

}