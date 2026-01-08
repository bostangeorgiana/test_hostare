using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Shared;

namespace CampusEats.Features.Admin.ManageAdmin.CreateAdmin;

public class CreateAdminHandler(UserRepository users)
{
    public async Task<Result<CreateAdminResponse>> Handle(
        CreateAdminCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(command.Password);

            // Create User entity
            var user = new User
            {
                FirstName = command.FirstName,
                LastName  = command.LastName,
                Email     = command.Email,
                Password  = hashedPassword,
                Role      = "admin"
            };

            // Will throw DuplicateEmailException if email exists
            var createdUser = await users.CreateUserAsync(user);

            // Build response DTO
            var response = new CreateAdminResponse
            {
                UserId     = createdUser.UserId,
                FirstName  = createdUser.FirstName,
                LastName   = createdUser.LastName,
                Email      = createdUser.Email,
                Role       = createdUser.Role,
                CreatedAt  = DateTime.UtcNow
            };

            return Result<CreateAdminResponse>.Success(response);
        }
        catch (Exception ex)
        {
            // Custom exceptions (DuplicateEmailException) will be handled by middleware
            return Result<CreateAdminResponse>.Failure(ex.Message);
        }
    }
}