using Microsoft.AspNetCore.Identity;

namespace Steria.Core.Interfaces;
public interface IDataSeederRepository
{
    Task SeedRolesAsync();
    Task SeedBodyStylesAsync();
    Task SeedNotificationTypesAsync();
    Task SeedBasicUsersAsync();
    Task SeedFollowsAsync();
    Task SeedMakeAsync();
    Task SeedModelAsync();
    Task SeedCarAuctionsAsync();
    Task SeedBidsAsync();
    Task SeedCommentsAsync();
    Task SeedQuestionAnswerAsync();
}