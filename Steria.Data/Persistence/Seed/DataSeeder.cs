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
            await seederRepository.SeedBasicUsersAsync();
            await seederRepository.SeedFollowsAsync();
            await seederRepository.SeedMakeAsync();
            await seederRepository.SeedModelAsync();
            await seederRepository.SeedCarAuctionsAsync();
            await seederRepository.SeedBidsAsync();
            await seederRepository.SeedCommentsAsync();
            await seederRepository.SeedQuestionAnswerAsync();
        }

    }

}