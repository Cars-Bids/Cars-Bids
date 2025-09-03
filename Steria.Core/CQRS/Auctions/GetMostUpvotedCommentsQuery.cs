using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetMostUpvotedCommentsQuery : IRequest<List<AuctionActivityDto>>
{
    public int AuctionId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetMostUpvotedCommentsQueryHandler(IGenericRepository<Comment> commentRepository,
                                                IMapper mapper) : IRequestHandler<GetMostUpvotedCommentsQuery, List<AuctionActivityDto>>
{
    public async Task<List<AuctionActivityDto>> Handle(GetMostUpvotedCommentsQuery request, CancellationToken cancellationToken)
    {
        var spec = new AuctionMostUpvotedCommentsSpec(request.AuctionId, request.PageNumber, request.PageSize);
        var comments = await commentRepository.GetListBySpec(spec, cancellationToken);
        return mapper.Map<List<AuctionActivityDto>>(comments);
    }
}