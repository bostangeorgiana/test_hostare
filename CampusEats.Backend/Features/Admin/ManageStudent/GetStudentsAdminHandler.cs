using CampusEats.Persistence.Repositories;
using CampusEats.Shared;

namespace CampusEats.Features.Admin.ManageStudent;

public class GetStudentsAdminHandler(StudentRepository students)
{
    public async Task<GetStudentsAdminResponse> GetList(
        int page,
        int pageSize,
        CancellationToken token)
    {
        PaginationValidator.Validate(page, pageSize);

        var total = await students.CountStudentsAsync();

        if (total == 0)
        {
            return new GetStudentsAdminResponse(
                Items: new(),
                TotalCount: 0,
                Page: page,
                PageSize: pageSize,
                TotalPages: 0,
                HasNextPage: false,
                HasPreviousPage: false
            );
        }

        var joinedRecords = await students.GetStudentsWithUserInfoAsync(page, pageSize);

        var items = joinedRecords.Select(x => new GetStudentsAdmin(
            x.UserId,
            x.FirstName,
            x.LastName,
            x.Email,
            x.LoyaltyPoints,
            x.Role
        )).ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return new GetStudentsAdminResponse(
            Items: items,
            TotalCount: total,
            Page: page,
            PageSize: pageSize,
            TotalPages: totalPages,
            HasNextPage: page < totalPages,
            HasPreviousPage: page > 1
        );
    }

    public async Task<Result<GetStudentsAdmin>> GetById(int id, CancellationToken token)
    {
        var result = await students.GetStudentWithUserInfoAsync(id);

        if (result == null)
            return Result<GetStudentsAdmin>.Failure("Student not found");

        return Result<GetStudentsAdmin>.Success(
            new GetStudentsAdmin(
                result.UserId,
                result.FirstName,
                result.LastName,
                result.Email,
                result.LoyaltyPoints,
                result.Role
            )
        );
    }
}
