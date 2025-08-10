using Microsoft.AspNetCore.Identity;

namespace Steria.Core.Entities;

public class User : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProfilePictureUrl { get; set; }

    public ICollection<Car>? AssingCars { get; set; }
    public ICollection<Car>? OwnedCars { get; set; }
    public ICollection<Auction>? Auctions { get; set; }
    public ICollection<Bid>? Bids { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<ChatMessage>? ChatMessages { get; set; }
    public ICollection<Question>? Questions { get; set; }
    public ICollection<Answer>? Answers { get; set; }
    public ICollection<Wishlist>? Wishlists { get; set; }
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
    public ICollection<Chat>? Chats { get; set; }
    public ICollection<UserChatMessageReaction>? UserChatMessageReactions { get; set; }
    public ICollection<UserNotificationSetting>? UserNotificationSettings { get; set; }
    public ICollection<UserNotification>? UserNotifications { get; set; }
    
}
