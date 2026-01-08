using CampusEats.Features.Menu.CreateMenuItem;
using MediatR;

namespace CampusEats.Features.Menu.UpdateMenuItem;

public class UpdateMenuItemCommand : IRequest<Unit>
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PictureLink { get; set; }
    public decimal Price { get; set; }
    public int Calories { get; set; }
    public int CurrentStock { get; set; }
    public bool IsAvailable { get; set; }

    public List<int> LabelIds { get; set; } = new();
    public List<MenuIngredientDto> Ingredients { get; set; } = new();
}