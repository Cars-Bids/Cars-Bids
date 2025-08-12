using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Steria.Data.Persistence.Seed;

namespace Steria.Data.Persistence.Repositories;

public class DataSeederRepository(
    ApplicationDbContext context,
    IGenericRepository<BodyStyle> bodyStyleRepository,
    IGenericRepository<NotificationType> notificationTypeRepository,
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

    public async Task SeedNotificationTypesAsync()
    {
        var seeder = new NotificationTypeSeed(notificationTypeRepository);
        await seeder.SeedAsync();
    }
}