using CampusEats.Persistence.Repositories;
using CampusEats.Shared;
using CampusEats.Shared.Exceptions;

namespace CampusEats.Features.Admin.ManageStudent;

public class ApproveStudentRequestHandler(
    UserRepository users,
    StudentRepository students)
{
    public async Task<Result> Handle(int userId, CancellationToken cancellationToken)
    {
        try
        {
            // Load user or throw UserNotFoundException
            var user = await users.GetByIdAsync(userId);

            // Validate that the user is pending
            if (user.Role != "Pending")
                throw new UserNotPendingException(userId);

            // Approve the request → set role to student
            user.Role = "student";
            await users.UpdateUserAsync(user); // We'll add this method below

            // Create student profile with loyalty points = 0
            await students.CreateStudentAsync(userId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}