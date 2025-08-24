namespace CarsAndBids.Core.DTOs;
public class CommentWithNameDto
{
public int Id { get; set; }
public int AuctionId { get; set; }
public int UserId { get; set; }
public string UserName { get; set; } = null!;
public string Text { get; set; } = null!;
public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}