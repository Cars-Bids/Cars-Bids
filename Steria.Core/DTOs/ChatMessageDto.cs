using System.Security.AccessControl;

namespace Steria.Core.DTOs;

public class ChatMessageDto
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string? Message { get; set; }
    public DateTime SentAt { get; set; }
    public List<string>? Attachment { get; set; }
    public List<ReactionSummaryDto>? ReactionSummaryDtos { get; set; }
    public List<SeenInfoDto>? SeenBy { get; set; }
}