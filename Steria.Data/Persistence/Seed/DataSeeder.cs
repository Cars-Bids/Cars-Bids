using Steria.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Steria.Data.Persistence.Seed
{
    public class DataSeeder(
        IDataSeederRepository seederRepository
        )
    {
        public async Task SeedAsync()
        {
            await seederRepository.SeedRolesAsync();
            await seederRepository.SeedBodyStylesAsync();
            await seederRepository.SeedNotificationTypesAsync();
        }

    }

}