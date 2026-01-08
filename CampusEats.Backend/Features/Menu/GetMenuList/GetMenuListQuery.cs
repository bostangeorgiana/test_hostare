using MediatR;
using CampusEats.Features.Menu;

namespace CampusEats.Features.Menu.GetMenuList;

public class GetMenuListQuery : IRequest<List<MenuItemDto>>
{
    public List<int>? Labels { get; set; }
    public AvailabilityFilter Availability { get; set; } = AvailabilityFilter.All;

    public bool OnlyFavorites { get; set; }

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}