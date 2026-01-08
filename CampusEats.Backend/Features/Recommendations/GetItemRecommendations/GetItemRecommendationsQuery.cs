using CampusEats.Features.Recommendations.Models;
using CampusEats.Shared;
using MediatR;

namespace CampusEats.Features.Recommendations.GetItemRecommendations;

public record GetItemRecommendationsQuery(int MenuItemId, int Limit = 3) : IRequest<Result<List<RecommendedMenuItem>>>;

