using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Chat;

public class CreateChatRequirementCommand : IRequest<ChatRequirementDto>
{
    public int ChatId { get; set; }
    public int ManagerId { get; set; }
    public string Text { get; set; }
}

public class CreateChatRequirementCommandHandler(IGenericRepository<ChatRequirements> requirementRepository, IMapper mapper) : IRequestHandler<CreateChatRequirementCommand, ChatRequirementDto>
{
    public async Task<ChatRequirementDto> Handle(CreateChatRequirementCommand request, CancellationToken cancellationToken)
    {
        var requirement = new ChatRequirements
            { ChatId = request.ChatId, CreatedById = request.ManagerId, Text = request.Text };

        await requirementRepository.InsertAsync(requirement);

        return mapper.Map<ChatRequirementDto>(requirement);
    }
}