using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class ChatMessagesSpec : Specification<ChatMessage, ChatMessage>
{
    public ChatMessagesSpec(int chatId, int page, int pageSize)
    {
        Query.Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        
        Query.Include(x => x.Attachments)
             .Include(x => x.UserChatMessageReactions)!
             .ThenInclude(x => x.EmojiReactions)
             .Select(x => x);
    }
}