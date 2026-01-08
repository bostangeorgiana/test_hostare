namespace CampusEats.Features.Menu.Interfaces;

public interface INotificationService
{
    Task NotifyAsync(IEnumerable<int> studentIds, string message);
}