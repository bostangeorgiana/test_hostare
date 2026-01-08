namespace CampusEats.Features.Payment.Process;

public record ProcessPaymentResponse(
    bool Success,
    string Message,
    decimal Amount,
    int AvailablePoints,
    int AppliedPoints,
    int EarnedPoints
);