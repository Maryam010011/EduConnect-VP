using Vp.Models;

namespace Vp.Interfaces;

public interface INotificationService
{
    event Action<Notification>? OnNewNotification;
    IEnumerable<Notification> GetForUser(Guid userId);
    void Push(Notification notification);
    void MarkAllAsRead(Guid userId);
    int GetUnreadCount(Guid userId);
}
