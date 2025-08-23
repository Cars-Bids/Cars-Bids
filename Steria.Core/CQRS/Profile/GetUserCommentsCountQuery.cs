using Steria.Core.Entities;
using Steria.Core.Interfaces;
using MediatR;
using Steria.Core.Specification.ProfileSpec;

namespace Steria.Core.CQRS.Profile;
public class GetUserCommentsCountQuery : IRequest<int>
{
    public int UserId { get; set; }

    public GetUserCommentsCountQuery(int userId)
    {
        UserId = userId;
    }
}

public class GetUserCommentsCountHandler(
    IGenericRepository<Comment> commentRepository
    ) : IRequestHandler<GetUserCommentsCountQuery, int>
{
    public async Task<int> Handle(GetUserCommentsCountQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserCommentsCountSpec(request.UserId);
        return await commentRepository.CountAsync(spec, cancellationToken);
    }
}