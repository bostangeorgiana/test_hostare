using CampusEats.Persistence.Repositories;
using CampusEats.Shared;
using CampusEats.Shared.Exceptions;

namespace CampusEats.Features.Admin.ManageKitchenStaff;

public class DeleteKitchenStaffHandler(UserRepository users)
{
    public async Task<Result> Handle(int userId, CancellationToken cancellationToken)
    {
        try
        {
            // Load user or throw UserNotFoundException
            var user = await users.GetByIdAsync(userId);

            if (user.Role != "kitchen_staff")
                throw new UserNotKitchenStaffException(userId);

            await users.DeleteUserAsync(user);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}