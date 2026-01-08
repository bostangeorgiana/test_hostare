using CampusEats.Features.Menu.Interfaces;
using MediatR;

namespace CampusEats.Features.Menu.UpdateStock;

public class UpdateStockHandler(
    IMenuRepository menuRepository,
    IFavoritesRepository favoritesRepository,
    INotificationService notificationService)
    : IRequestHandler<UpdateStockCommand>
{
    public async Task Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        await menuRepository.UpdateStockAsync(request.MenuItemId, request.Stock);

        if (request.Stock == 0)
        {
            var students = await favoritesRepository.GetStudentsWhoFavoritedAsync(request.MenuItemId);
            await notificationService.NotifyAsync(students, "An item you favorited is now out of stock!");
        }
    }
}