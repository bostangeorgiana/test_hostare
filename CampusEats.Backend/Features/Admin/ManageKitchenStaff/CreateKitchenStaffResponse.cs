namespace CampusEats.Features.Admin.ManageKitchenStaff;

public record KitchenStaffResponse
{
    public int UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName  { get; init; } = string.Empty;
    public string Email     { get; init; } = string.Empty;
    public string Role      { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}