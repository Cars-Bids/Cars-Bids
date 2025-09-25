using MediatR;
using System.Net;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
using System;
namespace Steria.Core.CQRS.Profile;
public class FollowUserCommand : IRequest
{
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
}
public class FollowUserCommandHandler(
IGenericRepository<User> userRepository,
IGenericRepository<UserFollow> followRepository
) : IRequestHandler<FollowUserCommand>
{
    public async Task Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        if (request.FollowerId == request.FollowingId)
            throw new HttpException(HttpStatusCode.BadRequest);//Resource.CannotFollowSelf, 
        var follower = await userRepository.GetByIdAsync(request.FollowerId)
        ?? throw new HttpException(HttpStatusCode.NotFound);//?? throw new HttpException(string.Format(Resource.UserNotFoundById, request.FollowerId), HttpStatusCode.NotFound);
        var following = await userRepository.GetByIdAsync(request.FollowingId)
        ?? throw new HttpException(HttpStatusCode.NotFound);// ?? throw new HttpException(string.Format(Resource.UserNotFoundById, request.FollowingId), HttpStatusCode.NotFound);
        var existingFollow = await followRepository.GetAsync(filter: f => f.FollowerId == request.FollowerId && f.FollowingId == request.FollowingId);
        if (existingFollow != null && existingFollow.Any())
            throw new HttpException( HttpStatusCode.BadRequest);//Resource.AlreadyFollowingUser,
        var newFollow = new UserFollow
        {
            FollowerId = request.FollowerId,
            FollowingId = request.FollowingId,
            CreatedAt = DateTime.UtcNow
        };
        await followRepository.InsertAsync(newFollow);
    }
}