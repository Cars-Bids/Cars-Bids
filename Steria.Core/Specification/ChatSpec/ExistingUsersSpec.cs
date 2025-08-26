using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class ExistingUsersSpec : Specification<Chat, int>
{
    public ExistingUsersSpec(List<int> userIds, int currentUserId)
    {
        Query
            .Where(chat =>
                chat.Participants.Any(p => p.Id == currentUserId) &&
                chat.Participants.Any(p => userIds.Contains(p.Id) && p.Id != currentUserId)
            )
            .AsNoTracking()
            .SelectMany(chat => chat.Participants
                .Where(p => p.Id != currentUserId && userIds.Contains(p.Id))
                .Select(p => p.Id)
            );
    }
}