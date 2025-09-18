namespace Steria.Core.Entities;

public class ChatRequirements
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    
    public int ChatId { get; set; }
    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; }
    public User User { get; set; }
    
}