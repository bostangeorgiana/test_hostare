using CampusEats.Features.Menu.Interfaces;
using MediatR;

namespace CampusEats.Features.Menu.Favorites;

public class ToggleFavoriteHandler(IFavoritesRepository favoritesRepository) : IRequestHandler<ToggleFavoriteCommand>
{
    public async Task Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        var exists = await favoritesRepository.ExistsAsync(request.StudentId, request.MenuItemId);

        if (exists)
            await favoritesRepository.RemoveAsync(request.StudentId, request.MenuItemId);
        else
            await favoritesRepository.AddAsync(request.StudentId, request.MenuItemId);
    }
}