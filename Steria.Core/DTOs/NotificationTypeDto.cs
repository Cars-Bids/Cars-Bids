using Steria.Core.Enums;

namespace Steria.Core.DTOs;

public class NotificationTypeDto
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;
    public string RedirectRoute { get; set; } = null!;
    public NotificationSource SourceType { get; set; }
    public string Description { get; set; } = null!;
    //public bool DefaultSendEmail { get; set; }
    //public bool DefaultSendSite { get; set; }
    public bool IsMandatory { get; set; }

}