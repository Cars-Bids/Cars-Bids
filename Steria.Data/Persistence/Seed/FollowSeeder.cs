using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class FollowSeeder(UserManager<User> userManager,
                          IGenericRepository<UserFollow> repository)
{
    public async Task SeedAsync()
    {
        var existing = await repository.GetItemBySpec(new FirstRecordSpec<UserFollow>());

        if (existing is not null) return;
        
        var allUsers = await userManager.Users.ToListAsync();

        var random = new Random();
        var follows = new List<UserFollow>();
        var existingFollows = new HashSet<(int, int)>();

        foreach (var follower in allUsers)
        {
            var others = allUsers.Where(u => u.Id != follower.Id).ToList();
            int numToFollow = random.Next(1, 6);
            if (others.Count < numToFollow) numToFollow = others.Count;

            var toFollow = others.OrderBy(_ => random.Next()).Take(numToFollow);

            foreach (var following in toFollow)
            {
                var key = (follower.Id, following.Id);
                if (!existingFollows.Contains(key))
                {
                    follows.Add(new UserFollow
                    {
                        FollowerId = follower.Id,
                        FollowingId = following.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                    existingFollows.Add(key);
                }
            }
        }

        if (follows.Any())
        {
            await repository.InsertRangeAsync(follows);
        }
    }
}