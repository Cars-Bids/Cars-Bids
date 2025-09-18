using Ardalis.Specification;
using Steria.Core.DTOs;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ChatSpec;

public class ChatRequirementsSpec : Specification<ChatRequirements, ChatRequirementDto>
{
    public ChatRequirementsSpec(int chatId)
    {
        Query.Where(x => x.ChatId == chatId)
            .Select(x => new ChatRequirementDto
            {
                Id = x.Id,
                Text = x.Text
            });
    }
}