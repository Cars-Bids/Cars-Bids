using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class GetAllChatMessagesSpec : Specification<ChatMessage, ChatMessage>
{
    public GetAllChatMessagesSpec(int chatId)
    {
        Query
            .Where(x => x.ChatId == chatId)
            .Include(x => x.Attachments)
            .Include(x => x.UserChatMessageReactions)!
            .ThenInclude(x => x.EmojiReactions);
    }
}