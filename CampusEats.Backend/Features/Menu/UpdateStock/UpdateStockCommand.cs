using MediatR;

namespace CampusEats.Features.Menu.UpdateStock;

public record UpdateStockCommand(int MenuItemId, int Stock) : IRequest;
