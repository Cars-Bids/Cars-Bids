using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.ChatSpec;

public class ChatContainsUserSpec : Specification<Chat, int>
{
    public ChatContainsUserSpec(int chatId, int userId)
    {
        Query.Where(c =>
            c.Id == chatId &&
            c.Participants.Any(p => p.Id == userId))
            .Select(chat => chat.Id);
    }
}