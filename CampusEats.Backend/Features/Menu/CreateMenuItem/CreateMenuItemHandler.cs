using CampusEats.Features.Menu.Interfaces;
using MediatR;

namespace CampusEats.Features.Menu.CreateMenuItem;

public class CreateMenuItemHandler(IMenuRepository menuRepo, IUnitOfWork uow)
    : IRequestHandler<CreateMenuItemCommand, int>
{
    public async Task<int> Handle(CreateMenuItemCommand request, CancellationToken ct)
    {
        var itemId = await menuRepo.CreateMenuItemAsync(
            request.Name,
            request.Description,
            request.Price,
            request.Calories,
            request.Stock,
            request.PictureLink
        );

        await menuRepo.AssignLabelsAsync(itemId, request.LabelIds);
        await menuRepo.AssignIngredientsAsync(itemId, request.Ingredients);

        await uow.SaveChangesAsync();

        return itemId;
    }
}