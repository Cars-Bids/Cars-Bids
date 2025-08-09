namespace CarsAndBids.Core.Entities;

public class ChatAttachment
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public ChatMessage Message { get; set; } = null!;
}