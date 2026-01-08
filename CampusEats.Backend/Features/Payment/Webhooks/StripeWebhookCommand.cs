using MediatR;
using CampusEats.Shared;

namespace CampusEats.Features.Payment.Webhooks;

public record StripeWebhookCommand(
    string Payload,
    string Signature
) : IRequest<Result>;