using MediatR;

namespace CampusEats.Features.Menu.GetIngredients;

public record GetIngredientsQuery() : IRequest<List<GetIngredientsResponse>>;