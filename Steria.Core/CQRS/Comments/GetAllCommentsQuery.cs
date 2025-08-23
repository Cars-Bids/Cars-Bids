using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using MediatR;

namespace Steria.Core.CQRS.Comments;

public class GetAllCommentsQuery : IRequest<List<CommentDto>> {}

public class GetAllCommentsHandler(
    IMapper mapper,
    IGenericRepository<Comment> repository
    ) : IRequestHandler<GetAllCommentsQuery, List<CommentDto>>
{
    public async Task<List<CommentDto>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
    {
        var Comment = await repository.GetAsync();

        return mapper.Map<List<CommentDto>>(Comment);
    }
}
