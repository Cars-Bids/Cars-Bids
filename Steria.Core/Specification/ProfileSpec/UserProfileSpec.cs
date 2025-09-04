using Ardalis.Specification;
using Steria.Core.DTOs;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ProfileSpec;

public class UserProfileSpec : Specification<User, ProfileDto>
{
    public UserProfileSpec(int userId)
    {
        Query
            .Where(user => user.Id == userId)
            .Include(user => user.Followers)
            .Include(user => user.Following)
            .AsNoTracking();

        Query.Select(user => new ProfileDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Bio = user.Bio,
            ProfilePictureUrl = user.ProfilePictureUrl,
            CreatedAt = user.CreatedAt,
            FollowersCount = user.Followers.Count,
            FollowingCount = user.Following.Count
        });
    }
}