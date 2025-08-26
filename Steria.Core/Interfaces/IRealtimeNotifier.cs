using Steria.Core.Enums;

namespace Steria.Core.Interfaces;

public interface IRealtimeNotifier
{
    Task SendToUserAsync(int userId, NotificationSource source, object payload);
}