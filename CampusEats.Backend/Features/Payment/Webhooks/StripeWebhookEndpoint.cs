using MediatR;

namespace CampusEats.Features.Payment.Webhooks;

public static class StripeWebhookEndpoint
{
    public static RouteHandlerBuilder MapStripeWebhook(this IEndpointRouteBuilder app)
    {
        return app.MapPost("/payments/webhook", async (
            HttpRequest request,
            IMediator mediator) =>
        {
            string json = await new StreamReader(request.Body).ReadToEndAsync();
            string signature = request.Headers["Stripe-Signature"]!;

            var cmd = new StripeWebhookCommand(json, signature);

            var result = await mediator.Send(cmd);

            return result.IsSuccess
                ? Results.Ok()
                : Results.BadRequest(result.Error);
        });
    }
}