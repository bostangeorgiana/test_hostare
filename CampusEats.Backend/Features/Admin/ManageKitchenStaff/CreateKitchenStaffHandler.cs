using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Shared;

namespace CampusEats.Features.Admin.ManageKitchenStaff;

public class CreateKitchenStaffHandler(UserRepository users)
{
    public async Task<Result<KitchenStaffResponse>> Handle(
        CreateKitchenStaffCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(command.Password);

            // Build User entity
            var user = new User
            {
                FirstName = command.FirstName,
                LastName  = command.LastName,
                Email     = command.Email,
                Password  = hashedPassword,
                Role      = "kitchen_staff"
            };

            // This will throw DuplicateEmailException if email already in use
            var created = await users.CreateUserAsync(user);

            var response = new KitchenStaffResponse
            {
                UserId    = created.UserId,
                FirstName = created.FirstName,
                LastName  = created.LastName,
                Email     = created.Email,
                Role      = created.Role,
                CreatedAt = DateTime.UtcNow
            };

            return Result<KitchenStaffResponse>.Success(response);
        }
        catch (Exception ex)
        {
            // Your middleware can log / map specific exceptions later
            return Result<KitchenStaffResponse>.Failure(ex.Message);
        }
    }
}