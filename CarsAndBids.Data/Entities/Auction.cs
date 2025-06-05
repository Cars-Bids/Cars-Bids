using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsAndBids.Data.Entities;

public class Auction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public required User Seller { get; set; }

    [Required]
    [StringLength(50)]
    public required string Make { get; set; }

    [Required]
    [StringLength(50)]
    public required string Model { get; set; }

    [Required]
    public int Year { get; set; }

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal StartingPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentBid { get; set; }

    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    public DateTime EndTime { get; set; }

    [Required]
    public required string Status { get; set; } = "Draft";

    public string? VIN { get; set; }

    public int Mileage { get; set; }
    public string? Location { get; set; }

    public List<string> PhotoUrls { get; set; } = [];
    public List<Bid> Bids { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
}
