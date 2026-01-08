using CampusEats.Features.Menu.Interfaces;
using MediatR;

namespace CampusEats.Features.Menu.GetMenuLabels;

public class GetMenuLabelsHandler(IMenuRepository repo)
    : IRequestHandler<GetMenuLabelsQuery, List<GetMenuLabelsResponse>>
{
    public async Task<List<GetMenuLabelsResponse>> Handle(
        GetMenuLabelsQuery req,
        CancellationToken ct)
    {
        return await repo.GetMenuLabelsAsync(ct);
    }
}