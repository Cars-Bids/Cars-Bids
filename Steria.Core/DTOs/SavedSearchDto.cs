namespace Steria.Core.DTOs;

public class SavedSearchDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? ModelId { get; set; }
    public string? ModelName { get; set; }
    public int MakeId { get; set; }
    public string? MakeName { get; set; }
    public AuctionFilteredDto? FirstAuction { get; set; }
    public int TotalMatchingAuctions { get; set; }
}