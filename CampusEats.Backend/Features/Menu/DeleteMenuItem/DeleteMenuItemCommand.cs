using MediatR;

namespace CampusEats.Features.Menu.DeleteMenuItem
{
    public record DeleteMenuItemCommand(int MenuItemId) : IRequest<Unit>;
}