using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class GetChatMessageWithUserReactionSpec : Specification<ChatMessage, ChatMessage>
{
    public GetChatMessageWithUserReactionSpec(int userId, int messageId)
    {
        Query
            .Where(msg => msg.Id == messageId)
            .Include(msg => msg.Attachments)
            .Include(msg => msg.UserChatMessageReactions
                .Where(reaction => reaction.UserId == userId));
    }
}