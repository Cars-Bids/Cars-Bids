using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.ChatSpec;

public class ChatMessagesCountSpec : CountSpec<ChatMessage>
{
    public ChatMessagesCountSpec(int chatId)
    {
        Query.Where(x => x.ChatId == chatId)
            .AsNoTracking();
    }
}