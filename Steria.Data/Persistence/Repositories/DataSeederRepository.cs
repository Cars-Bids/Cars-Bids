using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Steria.Data.Persistence.Seed;

namespace Steria.Data.Persistence.Repositories;

public class DataSeederRepository(
    IGenericRepository<BodyStyle> bodyStyleRepository,
    IGenericRepository<NotificationType> notificationTypeRepository,
    RoleManager<IdentityRole<int>> roleManager,
    IGenericRepository<User> userRepository,
    IGenericRepository<UserNotificationSetting> settingRepository,
    UserManager<User> userManager,
    IGenericRepository<UserFollow> userFollowRepository,
    IGenericRepository<Make> makeRepository,
    IGenericRepository<Model> modelRepository,
    IGenericRepository<Car> carRepository,
    IGenericRepository<Auction> auctionRepository,
    IGenericRepository<Bid> bidRepository,
    IGenericRepository<Comment> commentRepository,
    IGenericRepository<Answer> answerRepository) : IDataSeederRepository
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

    public async Task SeedNotificationTypesAsync()
    {
        var seeder = new NotificationTypeSeed(notificationTypeRepository);
        await seeder.SeedAsync();
    }

    public async Task SeedBasicUsersAsync()
    {
        var seeder = new UserSeeder(userRepository, userManager, notificationTypeRepository, settingRepository);
        await seeder.SeedAsync();
    }

    public async Task SeedFollowsAsync()
    {
        var seeder = new FollowSeeder(userManager, userFollowRepository);
        await seeder.SeedAsync();
    }

    public async Task SeedMakeAsync()
    {
        var seeder = new MakeSeeder(makeRepository);
        await seeder.SeedAsync();
    }

    public async Task SeedModelAsync()
    {
        var seeder = new ModelSeeder(makeRepository, modelRepository);
        await seeder.SeedAsync();
    }

    public async Task SeedCarAuctionsAsync()
    {
        var seeder = new CarAuctionSeeder(carRepository, auctionRepository, modelRepository, bodyStyleRepository, userManager);
        await seeder.SeedAsync();
    }

    public async Task SeedBidsAsync()
    {
        var seeder = new BidSeeder(bidRepository, auctionRepository, userManager);
        await seeder.SeedAsync();
    }

    public async Task SeedCommentsAsync()
    {
        var seeder = new CommentSeeder(auctionRepository, commentRepository,userManager);
        await seeder.SeedAsync();
    }

    public async Task SeedQuestionAnswerAsync()
    {
        var seeder = new QuestionAnswerSeeder(auctionRepository, answerRepository, userManager);
        await seeder.SeedAsync();
    }
}