using CampusEats.Persistence.Repositories;
using CampusEats.Shared;
using CampusEats.Shared.Exceptions;

namespace CampusEats.Features.Admin.ManageStudent;

public class RejectStudentRequestHandler(UserRepository users)
{
    public async Task<Result> Handle(int userId, CancellationToken cancellationToken)
    {
        try
        {
            // Load user or throw UserNotFoundException
            var user = await users.GetByIdAsync(userId);

            // Must be pending
            if (user.Role != "Pending")
                throw new UserNotPendingException(userId);

            // Reject = remove the pending user entirely
            await users.DeleteUserAsync(user);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}