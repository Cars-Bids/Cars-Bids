namespace CarsAndBids.Data.Entities;

public class Question
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public string QuestionText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Auction Auction { get; set; }
    public User User { get; set; }
    public Answer Answer { get; set; }
}