using MediatR;

namespace Steria.Core.CQRS.Auctions;

public class GetNewestActivityQuery : IRequest
{
    public int AuctionId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
