using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.ChatSpec;

public class UnreadMessagesCountSpec : CountSpec<ChatMessage>
{
    public UnreadMessagesCountSpec(int chatId, int userId)
    {
        Query
            .Where(m => m.ChatId == chatId &&
                        m.SenderId != userId &&
                        !m.UserChatMessageReactions.Any(r => r.UserId == userId))
            .AsNoTracking();
    }
}