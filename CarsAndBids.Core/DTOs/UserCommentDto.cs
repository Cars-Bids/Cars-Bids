namespace CarsAndBids.Core.DTOs;

public class UserCommentDto
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}