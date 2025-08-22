using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.Profile;
using MediatR;

namespace CarsAndBids.Core.CQRS.Profile;

public class GetUserCommentsQuery : IRequest<PagedResult<UserCommentDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetUserCommentsQuery(int userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetUserCommentsHandler(
    IGenericRepository<Comment> commentRepository,
    IMapper mapper
    ) : IRequestHandler<GetUserCommentsQuery, PagedResult<UserCommentDto>>
{
    public async Task<PagedResult<UserCommentDto>> Handle(GetUserCommentsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserCommentsSpec(request.UserId, request.PageNumber, request.PageSize);
        var comments = await commentRepository.GetListBySpec<Comment>(spec, cancellationToken);

        var commentDtos = mapper.Map<List<UserCommentDto>>(comments);

        var countSpec = new UserCommentsCountSpec(request.UserId);
        var totalCount = await commentRepository.CountAsync(countSpec, cancellationToken);

        return new PagedResult<UserCommentDto>
        {
            Items = commentDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}