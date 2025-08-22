using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Steria.Core.Entities;

namespace Steria.Core.Specification.NotificationTypeSpec;

public class GetNotifTypeByKeySpec : Specification<NotificationType, NotificationType>
{
    public GetNotifTypeByKeySpec(string key)
    {
        Query.Where(n => n.Key == key)
             .Select(n => n);
    }
}