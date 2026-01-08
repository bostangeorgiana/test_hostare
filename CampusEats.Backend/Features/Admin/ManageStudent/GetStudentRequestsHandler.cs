using CampusEats.Persistence.Repositories;
using CampusEats.Shared;

namespace CampusEats.Features.Admin.ManageStudent;
public class GetStudentRequestsHandler(UserRepository users)
{
    public async Task<GetStudentRequestsResponse> Handle(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        PaginationValidator.Validate(page, pageSize);

        var total = await users.CountUsersByRoleAsync("Pending");
        var pending = await users.GetUsersByRoleAsync("Pending", page, pageSize);

        var items = pending.Select(u => new StudentRequest
        {
            UserId = u.UserId,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Status = u.Role
        }).ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return new GetStudentRequestsResponse(
            Items: items,
            TotalCount: total,
            Page: page,
            PageSize: pageSize,
            TotalPages: totalPages,
            HasNextPage: page < totalPages,
            HasPreviousPage: page > 1
        );
    }
}