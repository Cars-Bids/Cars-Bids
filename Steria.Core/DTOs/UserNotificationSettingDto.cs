using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steria.Core.Entities;

namespace Steria.Core.DTOs;
public class UserNotificationSettingDto
{
    public int Id { get; set; }
    public int NotificationTypeId { get; set; }
    public int UserId { get; set; }
    public bool SendEmail { get; set; }
    public bool SendInSite { get; set; }

    //public User User { get; set; } = null!;
    public NotificationTypeDto NotificationType { get; set; } = null!;
}
