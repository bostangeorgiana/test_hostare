using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Shared;
using FluentValidation;

namespace CampusEats.Features.Auth.Register;

public class RegisterHandler(
    UserRepository users,
    IValidator<RegisterCommand> validator,
    ILogger<RegisterHandler> logger)
{
    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
        {
            var errorString = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return Result<RegisterResponse>.Failure(errorString);
        }
        
        var newUser = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(command.Password),
            Role = "Pending"
        };

        newUser = await users.CreateUserAsync(newUser);

        logger.LogInformation("New student registered (Pending): {UserId}", newUser.UserId);

        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                newUser.UserId,
                newUser.FirstName,
                newUser.LastName,
                newUser.Email,
                newUser.Role
            )
        );
    }
}