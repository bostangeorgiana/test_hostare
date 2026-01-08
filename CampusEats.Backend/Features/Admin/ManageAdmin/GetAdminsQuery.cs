namespace CampusEats.Features.Admin.ManageAdmin;

public record GetAdminsQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}