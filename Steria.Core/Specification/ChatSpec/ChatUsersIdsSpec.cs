using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class ChatUsersIdsSpec : Specification<Chat, List<int>>
{
    public ChatUsersIdsSpec(int chatId)
    {
        Query
            .Where(c => c.Id == chatId)
            // вибираємо тільки айді учасників
            .Select(c => c.Participants.Select(p => p.Id).ToList());
    }
}