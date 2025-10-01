using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class ChatUnreadMessageSpec : Specification<ChatMessage, int>
{
    public ChatUnreadMessageSpec(int chatId, int userId)
    {
        Query
            .Where(m => m.ChatId == chatId && 
                        m.SenderId != userId &&
                        !m.UserChatMessageReactions.Any(r => r.UserId == userId))
            .Select(x => x.Id);
    }
}