using System.Collections.Concurrent;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.ChatSpec;
using MediatR;

namespace CarsAndBids.Core.CQRS.Chat;

public class GetConnectedUsersIdQuery : IRequest<List<int>>
{
    public int CurrentUserId { get; set; }
    public List<int> TargetUserIds { get; set; }
}

public class GetConnectedUsersIdQueryHandler(IGenericRepository<Entities.Chat> chatRepository) : IRequestHandler<GetConnectedUsersIdQuery, List<int>>
{
    public async Task<List<int>> Handle(GetConnectedUsersIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ExistingUsersSpec(request.TargetUserIds, request.CurrentUserId);
        return await chatRepository.GetListBySpec(spec, cancellationToken);
    }
}