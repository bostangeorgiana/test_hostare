using CampusEats.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Features.Menu.GetIngredients;

public class GetIngredientsHandler(CampusEatsDbContext db)
    : IRequestHandler<GetIngredientsQuery, List<GetIngredientsResponse>>
{
    public async Task<List<GetIngredientsResponse>> Handle(GetIngredientsQuery request, CancellationToken ct)
    {
        return await db.Ingredients
            .OrderBy(i => i.Name)
            .Select(i => new GetIngredientsResponse(
                i.IngredientId,
                i.Name,
                i.Allergens,
                i.CaloriesPerUnit
            ))
            .ToListAsync(ct);
    }
}