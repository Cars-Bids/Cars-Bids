using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.ProfileSpec;
using MediatR;

namespace CarsAndBids.Core.CQRS.Profile;

public class GetUserAuctionCommentsQuery : IRequest<PagedResult<CommentWithNameDto>>
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
    ) : IRequestHandler<GetUserAuctionCommentsQuery, PagedResult<CommentWithNameDto>>
{
    public async Task<PagedResult<CommentWithNameDto>> Handle(GetUserAuctionCommentsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserAuctionCommentsSpec(request.UserId, request.PageNumber, request.PageSize);
        var comments = await commentRepository.GetListBySpec(spec, cancellationToken);

        var CommentWithNameDtos = mapper.Map<List<CommentWithNameDto>>(comments);

        var totalCount = await commentRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<CommentWithNameDto>
        {
            Items = CommentWithNameDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}