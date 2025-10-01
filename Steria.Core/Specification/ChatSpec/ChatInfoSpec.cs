using Ardalis.Specification;
using Steria.Core.DTOs;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class ChatInfoSpec : Specification<Chat, ChatInfoDto>
{
    public ChatInfoSpec(int chatId, int userId)
    {
        Query
            .Where(x => x.Id == chatId)
            .Select(x => new ChatInfoDto
            {
                Make = x.Car.Model.Make.Name,
                Model = x.Car.Model.Name,
                Year = x.Car.Year,
                Username = x.Participants
                    .Where(u => u.Id != userId)
                    .Select(u => u.UserName)
                    .FirstOrDefault()
            });
    }
}
