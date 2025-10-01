namespace Steria.Core.DTOs;

public class AuctionDetailedDto
{
    public AuctionData Auction { get; set; }
    public CarData Car { get; set; }
    public List<CarImageData> Images { get; set; } = [];
    public List<QAData> Qa { get; set; } = [];
    public List<CommentData> Comments { get; set; } = [];
    public List<OtherAuction> Auctions { get; set; } = [];
}

public class OtherAuction
{
    public required int Id { get; set; }
    public required string? MainPhoto { get; set; }
    public required decimal CurrentPrice { get; set; }
    public required DateTime? StartTime { get; set; }
    public required DateTime? EndTime { get; set; }
    public required bool IsWatched { get; set; }
    public required bool IsInspected { get; set; }
    public required string? Location { get; set; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
}

public class AuctionData
{
    public required int Id { get; set; }
    public required int CarId { get; set; }
    public required int SellerId { get; set; }
    public required string Status { get; set; }
    public required string? Seller { get; set; }
    public required string? SellerPhoto { get; set; }
    public required decimal CurrentPrice { get; set; }
    public required int? CurrentBidderId { get; set; }
    public required string? CurrentBidder { get; set; }
    public required string? CurrentBidderPhoto { get; set; }
    public required DateTime? StartTime { get; set; }
    public required DateTime? EndTime { get; set; }
    public required int BidsCount { get; set; }
    public required int ViewsCount { get; set; }
    public required int WatchersCount { get; set; }
    public required bool IsWatched { get; set; }
    public required bool IsSeller { get; set; }
    public required bool IsInspected { get; set; }
}

public class CarData
{
    public required int Id { get; set; }
    public required int Year { get; set; }
    public required string Model { get; set; } = null!;
    public required string Brand { get; set; } = null!;
    public required string Vin { get; set; } = null!;
    public required int Mileage { get; set; }
    public required string? Location { get; set; }
    public required string? Seller { get; set; }
    public required string? SellerPhoto { get; set; }
    public required string? Engine { get; set; }
    public required string? Drivetrain { get; set; }
    public required string TransmissionType { get; set; }
    public required string? BodyStyle { get; set; }
    public required string? ExteriorColor { get; set; }
    public required string? InteriorColor { get; set; }
    public required string? TitleStatus { get; set; }
    public required string? SellerType { get; set; }
    public required string[] VideoLinks { get; set; }


    public required string? Highlights { get; set; }
    public required string? ServiceHistory { get; set; }
    public required string? Equipment { get; set; }
    public required string? Flaws { get; set; }
    public required string? Modifications { get; set; }
    public required string? OtherItems { get; set; }
    public required string? OwnershipHistory { get; set; }
    public required string? SellerNotes { get; set; }

    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string? About { get; set; }
}

public class CarImageData
{
    public required int Id { get; set; }
    public required string ImageUrl { get; set; } = null!;
    public required string ImageCategory { get; set; }
}

public class QAData
{
    public required int Id { get; set; }
    public required string Question { get; set; } = null!;
    public required string? Answer { get; set; }
    public required int AuthorId { get; set; }
    public required string? Author { get; set; }
    public required string? AuthorPhoto { get; set; }
}

public class CommentData
{
    public required int Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string Text { get; set; } = null!;
    public required string? ReplyTo { get; set; }
    public required int AuthorId { get; set; }
    public required decimal Bid { get; set; }
    public required string? Author { get; set; }
    public required string? AuthorPhoto { get; set; }
    public required int Upvotes { get; set; }    
}
