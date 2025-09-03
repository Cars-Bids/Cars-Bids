using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetAuctionSellerCommentsQuery : IRequest<List<AuctionActivityDto>>
{
    public int AuctionId { get; set; }
    public int SellerId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetAuctionSellerCommentsQueryHandler(IGenericRepository<Comment> commentRepository,
                                                  IMapper mapper) : IRequestHandler<GetAuctionSellerCommentsQuery, List<AuctionActivityDto>>
{
    public async Task<List<AuctionActivityDto>> Handle(GetAuctionSellerCommentsQuery request, CancellationToken cancellationToken)
    {
        var spec = new AuctionSellerCommentsSpec(request.AuctionId, request.SellerId, request.PageNumber, request.PageSize);
        var comment = await commentRepository.GetListBySpec(spec, cancellationToken);
        return mapper.Map<List<AuctionActivityDto>>(comment);
    }
}