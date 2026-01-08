using MediatR;

namespace CampusEats.Features.Menu.GetMenuItemById;

public record GetMenuItemByIdQuery(int MenuItemId) : IRequest<MenuItemDto>;