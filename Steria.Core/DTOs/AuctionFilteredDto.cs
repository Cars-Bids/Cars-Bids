namespace Steria.Core.DTOs;

public class AuctionFilteredDto
{
    public int AuctionId { get; set; }
    public string? MainImage { get; set; }
    public int Year { get; set; }
    public int ModelId { get; set; }
    public string? ModelName { get; set; }
    public int MakeId { get; set; }
    public string? MakeName { get; set; }
    public bool Inspected { get; set; }
    public string? Transmission { get; set; }
    public int Mileage { get; set; }
    public int NumberOfGears { get; set; }
    public string? Engine { get; set; }
    public string? BodyStyleName { get; set; }
    public string? Interior { get; set; }
    public string? Exterior { get; set; }
    public string? Location { get; set; }
    public DateTime? EndTime { get; set; } = null!;
    public decimal? CurrentBid { get; set; }
}