using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class ChatRequirementsQuery : IRequest<List<ChatRequirementDto>>
{
    public int ChatId { get; set; }
}

public class ChatRequirementsQueryHandler(IGenericRepository<ChatRequirements> requirementsRepository) : IRequestHandler<ChatRequirementsQuery, List<ChatRequirementDto>>
{
    public async Task<List<ChatRequirementDto>> Handle(ChatRequirementsQuery request, CancellationToken cancellationToken)
    {
        return await requirementsRepository.GetListBySpec(new ChatRequirementsSpec(request.ChatId), cancellationToken);
    }
}