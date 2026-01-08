using MediatR;

namespace CampusEats.Features.Menu.GetMenuLabels;

public record GetMenuLabelsQuery
    : IRequest<List<GetMenuLabelsResponse>>;