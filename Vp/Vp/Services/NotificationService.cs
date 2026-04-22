using Vp.Interfaces;
using Vp.Models;

namespace Vp.Services;

public sealed class NotificationService(DataStore store) : INotificationService
{
    public event Action<Notification>? OnNewNotification;

    public IEnumerable<Notification> GetForUser(Guid userId) =>
        store.Notifications.Where(n => n.RecipientId == userId).OrderByDescending(n => n.CreatedAt);

    public void Push(Notification notification)
    {
        store.Notifications.Add(notification);
        OnNewNotification?.Invoke(notification);
    }

    public void MarkAllAsRead(Guid userId)
    {
        foreach (var item in store.Notifications.Where(n => n.RecipientId == userId))
        {
            item.IsRead = true;
        }
    }

    public int GetUnreadCount(Guid userId) => store.Notifications.Count(n => n.RecipientId == userId && !n.IsRead);
}
