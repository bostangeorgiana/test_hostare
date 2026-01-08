using FluentValidation;

namespace CampusEats.Features.Admin.ManageKitchenStaff;

public class CreateKitchenStaffValidator : AbstractValidator<CreateKitchenStaffCommand>
{
    public CreateKitchenStaffValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100);
    }
}