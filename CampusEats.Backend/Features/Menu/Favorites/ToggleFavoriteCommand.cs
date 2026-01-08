using MediatR;

namespace CampusEats.Features.Menu.Favorites;

public record ToggleFavoriteCommand(int StudentId, int MenuItemId) : IRequest;