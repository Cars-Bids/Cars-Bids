using Steria.Core.Enums;

namespace Steria.Core.Entities;

public class Chat
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User>? Participants { get; set; }
    public ICollection<ChatMessage>? Messages { get; set; }
    public Car? Car { get; set; }
}