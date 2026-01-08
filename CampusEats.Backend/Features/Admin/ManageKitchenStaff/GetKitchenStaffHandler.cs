using CampusEats.Persistence.Repositories;
using CampusEats.Shared;

namespace CampusEats.Features.Admin.ManageKitchenStaff;
public class GetKitchenStaffHandler(UserRepository users)
{
    public async Task<GetKitchenStaffResponse> Handle(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        PaginationValidator.Validate(page, pageSize);

        var total = await users.CountUsersByRoleAsync("kitchen_staff");
        var kitchenStaff = await users.GetUsersByRoleAsync("kitchen_staff", page, pageSize);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        var items = kitchenStaff.Select(u => new KitchenStaff
        {
            UserId = u.UserId,
            FullName = $"{u.FirstName} {u.LastName}",
            Email = u.Email,
            Role = u.Role
        }).ToList();

        return new GetKitchenStaffResponse(
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