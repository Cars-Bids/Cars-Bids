using MediatR;
using System.Net;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
using AutoMapper;

namespace Steria.Core.CQRS.Profile;

public class GetIsFollowingQuery : IRequest<bool>
{
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
}

public class GetIsFollowingHandler(
    IGenericRepository<UserFollow> followRepository
    ) : IRequestHandler<GetIsFollowingQuery, bool>
{
    public async Task<bool> Handle(GetIsFollowingQuery request, CancellationToken cancellationToken)
    {
        var follows = await followRepository.GetAsync(f => f.FollowerId == request.FollowerId && f.FollowingId == request.FollowingId);
        return follows.Any();
    }
}   