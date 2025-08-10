namespace Steria.Core.Entities;

public class UserChatMessageReaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ChatMessageId { get; set; }
    public DateTime SeenAt { get; set; } = DateTime.UtcNow;

    public ChatMessage ChatMessage { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<EmojiReaction>? EmojiReactions { get; set; }
}