using CampusEats.Features.Menu.Interfaces;

namespace CampusEats.Backend.Menu.Services
{
    public class NotificationService : INotificationService
    {
        public Task NotifyAsync(IEnumerable<int> studentIds, string message)
        {
            // For now, just log to console. In real app, you might send emails or push notifications.
            foreach (var studentId in studentIds)
            {
                Console.WriteLine($"Notifying student {studentId}: {message}");
            }
            return Task.CompletedTask;
        }
    }
}