using CampusEats.Shared;
using MediatR;

namespace CampusEats.Features.Payment.Process;

public record ProcessPaymentCommand(int OrderId, string PaymentMethodId, int PointsToRedeem = 0)
    : IRequest<Result<ProcessPaymentResponse>>;