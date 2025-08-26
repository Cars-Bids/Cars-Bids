using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class GetChatMessageReactionSpec : Specification<UserChatMessageReaction, UserChatMessageReaction>
{
    public GetChatMessageReactionSpec(int userId, int messageId)
    {
        Query
            .Where(x => x.UserId == userId && x.ChatMessageId == messageId)
            .Include(x => x.EmojiReactions);
    }
}