using MediatR;

namespace CampusEats.Features.Menu.Favorites;

public record GetFavoriteMenuItemsByStudentCommand(int StudentId)
    : IRequest<List<MenuItemDto>>;