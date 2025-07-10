namespace CarsAndBids.Core.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string? Message { get; set; }
    public bool HasAttachments { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<ChatAttachment>? Attachments { get; set; }
}