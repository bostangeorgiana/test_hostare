using CampusEats.Persistence.Repositories;
using CampusEats.Shared;

namespace CampusEats.Features.Admin.ManageAdmin;

public class GetAdminsHandler(UserRepository users)
{
    public async Task<Result<GetAdminsPagedResponse>> Handle(
        GetAdminsQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate page + pageSize
            PaginationValidator.Validate(query.Page, query.PageSize);

            var page = query.Page;
            var pageSize = query.PageSize;

            // Count total admins
            var total = await users.CountUsersByRoleAsync("admin");

            // Paginated query
            var admins = await users.GetUsersByRoleAsync("admin", page, pageSize);

            var items = admins.Select(a => new GetAdmins
            {
                UserId = a.UserId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                Role = a.Role
            }).ToList();

            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var response = new GetAdminsPagedResponse(
                Items: items,
                TotalCount: total,
                Page: page,
                PageSize: pageSize,
                TotalPages: totalPages,
                HasNextPage: page < totalPages,
                HasPreviousPage: page > 1
            );

            return Result<GetAdminsPagedResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GetAdminsPagedResponse>.Failure(ex.Message);
        }
    }
}