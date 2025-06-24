namespace CarsAndBids.Data.Entities;

public class Chat
{
    public int Id { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public bool IsAuctionChat { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ChatMessage>? Messages { get; set; }
    public User User1 { get; set; } = null!;
    public User User2 { get; set; } = null!;
    public Car? Car { get; set; }
}