using FluentValidation;

namespace CampusEats.Features.Payment.Process;

public class ProcessPaymentValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithMessage("OrderId must be a positive integer.");

        RuleFor(x => x.PaymentMethodId)
            .NotEmpty()
            .WithMessage("PaymentMethodId is required.");

        RuleFor(x => x.PointsToRedeem)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PointsToRedeem cannot be negative.");
    }
}