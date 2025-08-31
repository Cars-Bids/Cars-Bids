using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Steria.Core.Constants;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class UserSeeder(IGenericRepository<User> userRepository,
                        UserManager<User> userManager,
                        IGenericRepository<NotificationType> notifTypeRepository,
                        IGenericRepository<UserNotificationSetting> settingRepository)
{
    public async Task SeedAsync()
    {
        var existing = await userRepository.GetItemBySpec(new FirstRecordSpec<User>());

        if (existing is null)
        {
            const string password = "Qwerty123!";
            
            var random = new Random();
            var firstNames = new List<string> { "Alex", "Jordan", "Taylor", "Casey", "Morgan", "Riley", "Jamie", "Quinn", "Avery", "Cameron", "Blake", "Drew", "Emery", "Finley", "Hayden", "Jesse", "Kendall", "Logan", "Parker", "Reese" };
            var lastNames = new List<string> { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };

            for (var i = 0; i < 20; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Count)];
                var lastName = lastNames[random.Next(lastNames.Count)];
                var username = $"{firstName}{lastName}{random.Next(1000)}".ToLower();
                var email = $"{username}@example.com";
                
                var existingUser = await userManager.FindByEmailAsync(email);
                if (existingUser != null)
                {
                    continue;
                }
                
                var usernameTaken = await userManager.FindByNameAsync(username);
                if (usernameTaken != null)
                {
                    continue;
                }
                
                var newUser = new User
                {
                    UserName = username,
                    Email = email
                };

                var result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, i == 1 ? Roles.Manager : Roles.User);
                    
                    var allNotifTypes = await notifTypeRepository.GetAsync();
                    var defaultSettings = allNotifTypes.Select(nt => new UserNotificationSetting
                    {
                        UserId = newUser.Id,
                        NotificationTypeId = nt.Id,
                        SendEmail = nt.DefaultSendEmail,
                        SendInSite = nt.DefaultSendSite
                    }).ToList();

                    await settingRepository.InsertRangeAsync(defaultSettings);
                }
                
            }
        }
    }
}