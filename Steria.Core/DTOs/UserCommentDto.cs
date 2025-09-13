namespace Steria.Core.DTOs;

public class UserCommentDto
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int Year { get; set; } // Car's year
    public string Make { get; set; } // Car's make
    public string Model { get; set; } // Car's model
    public string BodyStyle { get; set; } // Car's body style
    public string MainImage { get; set; } // URL of the main image
}