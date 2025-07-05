using CarsAndBids.Data.Enums;

namespace CarsAndBids.Data.Entities;

public class Chat
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User>? Participants { get; set; }
    public ICollection<ChatMessage>? Messages { get; set; }
    public Car? Car { get; set; }
}