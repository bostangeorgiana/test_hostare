namespace CampusEats.Features.Admin.ManageKitchenStaff;

public record GetKitchenStaffResponse(
    List<KitchenStaff> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
);

public record KitchenStaff
{
    public int UserId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}