using CampusEats.Features.Menu.Interfaces;
using MediatR;

namespace CampusEats.Features.Menu.GetMenuList;

public class GetMenuListHandler(IMenuRepository repo)
    : IRequestHandler<GetMenuListQuery, List<MenuItemDto>>
{
    public Task<List<MenuItemDto>> Handle(GetMenuListQuery query, CancellationToken ct)
        => repo.GetMenuListAsync(
            query.Labels,
            query.Availability,
            query.OnlyFavorites,
            query.MinPrice,
            query.MaxPrice
        );
}