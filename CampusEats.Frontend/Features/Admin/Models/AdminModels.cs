namespace CampusEats.Frontend.Features.Admin.Models;

public class StudentRequest
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Status { get; set; } = "";

    // Computed for UI
    public int Id => UserId;
    public string Name => $"{FirstName} {LastName}";
}

public class StudentRequestsResponse
{
    public List<StudentRequest> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class KitchenStaffResponse
{
    public List<KitchenStaffUser> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class KitchenStaffUser
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}