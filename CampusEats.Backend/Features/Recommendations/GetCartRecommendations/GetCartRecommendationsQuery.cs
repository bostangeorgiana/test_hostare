
using MediatR;
using CampusEats.Shared;
using CampusEats.Features.Recommendations.Models;

namespace CampusEats.Features.Recommendations.GetCartRecommendations;

public record GetCartRecommendationsQuery(List<int> ItemIds, int Limit = 3) : IRequest<Result<List<RecommendedMenuItem>>>;

