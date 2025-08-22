using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Comments;

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
