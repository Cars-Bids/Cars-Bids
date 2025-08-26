namespace Steria.Core.DTOs;

public class ReactionSummaryDto
{
    public string Emoji { get; set; } = null!;
    public int Count { get; set; }
    public bool ReactedByCurrentUser { get; set; }
}