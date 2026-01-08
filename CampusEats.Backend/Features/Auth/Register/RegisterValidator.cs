using CampusEats.Persistence.Repositories;
using FluentValidation;

namespace CampusEats.Features.Auth.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator(UserRepository users)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100)
            .Matches("^[A-Z][a-zA-Z '-]*$")
            .WithMessage("First name must start with a capital letter and contain only letters, spaces, hyphens or apostrophes.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100)
            .Matches("^[A-Z][a-zA-Z '-]*$")
            .WithMessage("Last name must start with a capital letter and contain only letters, spaces, hyphens or apostrophes.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
        
        RuleFor(x => x.Email)
            .MustAsync(async (email, ct) =>
            {
                var existing = await users.GetByEmailAsync(email);
                return existing == null;
            })
            .WithMessage("Email is already registered.");
    }
}