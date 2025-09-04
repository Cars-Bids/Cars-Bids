using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetNewestActivityQuery : IRequest<List<AuctionActivityDto>>
{
    public int AuctionId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetNewestActivityQueryHandler(IGenericRepository<Auction> auctionRepository) : IRequestHandler<GetNewestActivityQuery, List<AuctionActivityDto>>
{
    public async Task<List<AuctionActivityDto>> Handle(GetNewestActivityQuery request, CancellationToken cancellationToken)
    {
        var spec = new AuctionActivityBaseSpec(request.AuctionId);
        var auction = await auctionRepository.GetItemBySpec(spec, cancellationToken);

        if (auction == null) return new List<AuctionActivityDto>();

        var comments = auction.Comments.Select(c => new AuctionActivityDto
        {
            Type = "Comment",
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            Text = c.Text,
            ReplyId = c.ReplyId,
            Upvotes = c.CommentUpvotes.Count,
            UserId = c.UserId,
            UserName = c.User.UserName
        });

        var bids = auction.Bids.Select(b => new AuctionActivityDto
        {
            Type = "Bid",
            Id = b.Id,
            CreatedAt = b.BidTime,
            Amount = b.BidAmount,
            BidderId = b.UserId,
            BidderName = b.User.UserName
        });
    
        return comments.Union(bids)
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
    }
}