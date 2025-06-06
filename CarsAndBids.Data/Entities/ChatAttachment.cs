namespace CarsAndBids.Data.Entities;

public class ChatAttachment
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public string ImageUrl { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public ChatMessage Message { get; set; }
}