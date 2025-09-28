using MediatR;
using System.Net;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
namespace Steria.Core.CQRS.Profile;
public class UnfollowUserCommand : IRequest
{
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
}
public class UnfollowUserCommandHandler(
IGenericRepository<User> userRepository,
IGenericRepository<UserFollow> followRepository
) : IRequestHandler<UnfollowUserCommand>
{
    public async Task Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        if (request.FollowerId == request.FollowingId)
            throw new HttpException(HttpStatusCode.BadRequest);//throw new HttpException(Resource.CannotUnfollowSelf, HttpStatusCode.BadRequest);
        var follower = await userRepository.GetByIdAsync(request.FollowerId)
        ?? throw new HttpException(HttpStatusCode.NotFound);//?? throw new HttpException(string.Format(Resource.UserNotFoundById, request.FollowerId), HttpStatusCode.NotFound);
        var following = await userRepository.GetByIdAsync(request.FollowingId)
        ?? throw new HttpException(HttpStatusCode.NotFound);//?? throw new HttpException(string.Format(Resource.UserNotFoundById, request.FollowingId), HttpStatusCode.NotFound);
        var existingFollow = await followRepository.GetAsync(filter: f => f.FollowerId == request.FollowerId && f.FollowingId == request.FollowingId);
        if (existingFollow == null || !existingFollow.Any())
            throw new HttpException(HttpStatusCode.BadRequest);//throw new HttpException(Resource.NotFollowingUser, HttpStatusCode.BadRequest);
        await followRepository.DeleteAsync(existingFollow.First());
    }
}