using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Chat;

public class DeleteChatRequirementCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteChatRequirementsCommandHandler(IGenericRepository<ChatRequirements> requirementRepository) : IRequestHandler<DeleteChatRequirementCommand>
{
    public async Task Handle(DeleteChatRequirementCommand request, CancellationToken cancellationToken)
    {
        await requirementRepository.DeleteAsync(request.Id);
    }
}