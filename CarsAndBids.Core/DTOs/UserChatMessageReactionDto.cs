namespace CarsAndBids.Core.DTOs;

public class UserChatMessageReactionDto
{
    public int UserId { get; set; }
    public int ChatMessageId { get; set; }
    public DateTime SeenAt { get; set; }
}