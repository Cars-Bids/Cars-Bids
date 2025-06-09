namespace CarsAndBids.Data.Entities;

public class Chat
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ChatMessage>? Messages { get; set; }
}