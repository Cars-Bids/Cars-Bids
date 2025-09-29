using Steria.Core.DTOs;

public class FilteredAuctionsPagedResult
{
    public List<AuctionFilteredDto> Items { get; set; } = new List<AuctionFilteredDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public List<int> ModelIds { get; set; } = new List<int>();
    public List<int> MakeIds { get; set; } = new List<int>();
}