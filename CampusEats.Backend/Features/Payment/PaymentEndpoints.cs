using CampusEats.Features.Payment.Analytics;
using CampusEats.Features.Payment.Process;

namespace CampusEats.Features.Payment;

public static class PaymentEndpoints
{
    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/payments/process", ProcessPaymentEndpoint.Handle);
        app.MapPaymentAnalyticsEndpoints();
        return app;
    }
}