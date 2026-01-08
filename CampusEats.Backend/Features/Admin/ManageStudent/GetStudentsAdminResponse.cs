namespace CampusEats.Features.Admin.ManageStudent;

public record GetStudentsAdmin(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    int LoyaltyPoints,
    string Role
);

public record GetStudentsAdminResponse(
    List<GetStudentsAdmin> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
);
