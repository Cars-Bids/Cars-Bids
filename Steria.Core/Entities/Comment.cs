using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Steria.Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public int? ReplyId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Auction Auction { get; set; } = null!;
    public User User { get; set; } = null!;
    public Comment? ReplyedTo { get; set; }
    public ICollection<Comment>? Replies { get; set; }
    public ICollection<CommentUpvote> CommentUpvotes { get; set; }
}
