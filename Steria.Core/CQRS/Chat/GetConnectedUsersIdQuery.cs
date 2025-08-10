using System.Collections.Concurrent;
using MediatR;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class GetConnectedUsersIdQuery : IRequest<List<int>>
{
    public int CurrentUserId { get; set; }
    public List<int> TargetUserIds { get; set; }
}

//Get users where current user have connections with others to notify them
public class GetConnectedUsersIdQueryHandler(IGenericRepository<Entities.Chat> chatRepository) : IRequestHandler<GetConnectedUsersIdQuery, List<int>>
{
    public async Task<List<int>> Handle(GetConnectedUsersIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ExistingUsersSpec(request.TargetUserIds, request.CurrentUserId);
        return await chatRepository.GetListBySpec(spec, cancellationToken);
    }
}