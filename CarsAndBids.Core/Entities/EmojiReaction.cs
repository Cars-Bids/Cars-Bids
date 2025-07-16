namespace CarsAndBids.Core.Entities;

public class EmojiReaction
{
    public int Id { get; set; }
    public string Emoji { get; set; } = null!;
    public int MessageReactionId { get; set; }

    public UserChatMessageReaction UserChatMessageReaction { get; set; }
}