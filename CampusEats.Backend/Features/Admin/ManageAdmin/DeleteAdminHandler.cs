using CampusEats.Persistence.Repositories;
using CampusEats.Shared;
using CampusEats.Shared.Exceptions;

namespace CampusEats.Features.Admin.ManageAdmin;

public class DeleteAdminHandler(UserRepository users)
{
    public async Task<Result> Handle(int userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await users.GetByIdAsync(userId);

            if (user.Role != "admin")
                throw new UserNotAdminException(userId);

            var adminCount = await users.CountUsersByRoleAsync("admin");

            if (adminCount <= 1)
                throw new CannotDeleteLastAdminException();

            await users.DeleteUserAsync(user);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}