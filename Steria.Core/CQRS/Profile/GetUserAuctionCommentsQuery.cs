using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ProfileSpec;
using MediatR;

namespace Steria.Core.CQRS.Profile;

public class GetUserAuctionCommentsQuery : IRequest<PagedResult<UserCommentDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetUserAuctionCommentsQuery(int userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetUserAuctionCommentsHandler(
    IGenericRepository<Comment> commentRepository,
    IMapper mapper
    ) : IRequestHandler<GetUserAuctionCommentsQuery, PagedResult<UserCommentDto>>
{
    public async Task<PagedResult<UserCommentDto>> Handle(GetUserAuctionCommentsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserAuctionCommentsSpec(request.UserId, request.PageNumber, request.PageSize);
        var comments = await commentRepository.GetListBySpec(spec, cancellationToken);

        var userCommentDtos = mapper.Map<List<UserCommentDto>>(comments);

        var totalCount = await commentRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<UserCommentDto>
        {
            Items = userCommentDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}