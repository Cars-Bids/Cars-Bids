using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Data.Entities;

public class User : IdentityUser<int>
{
    public string? ProfilePictureUrl { get; set; }

    public List<Auction> Auctions { get; set; } = [];
    public List<Bid> Bids { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
}
