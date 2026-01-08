using CampusEats.Features.Menu.Interfaces;
using CampusEats.Shared.Exceptions;
using MediatR;

namespace CampusEats.Features.Menu.DeleteMenuItem
{
    public class DeleteMenuItemHandler(IMenuRepository menuRepo) : IRequestHandler<DeleteMenuItemCommand, Unit>
    {
        private readonly IMenuRepository _menuRepo = menuRepo;

        public async Task<Unit> Handle(DeleteMenuItemCommand request, CancellationToken ct)
        {
            var items = await _menuRepo.GetMenuItemsByIdsAsync(new List<int> { request.MenuItemId }, ct);
            if (!items.ContainsKey(request.MenuItemId))
                throw new NotFoundException($"Menu item {request.MenuItemId} does not exist.");
            
            await _menuRepo.DeleteMenuItemAsync(request.MenuItemId);

            return Unit.Value;
        }
    }
}