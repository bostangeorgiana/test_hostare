using Stripe;
using MediatR;
using CampusEats.Shared;
using System.Text.Json.Nodes;
using CampusEats.Persistence.Repositories;
using CampusEats.Persistence.Entities;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Features.Orders;

namespace CampusEats.Features.Payment.Webhooks;

public class StripeWebhookHandler(
    IConfiguration config,
    PaymentRepository payments,
    PaymentEventRepository eventsRepo,
    IOrderRepository orders,
    IMenuRepository menuRepo,
    StudentRepository students,
    IUnitOfWork uow,
    ILogger<StripeWebhookHandler> logger)
    : IRequestHandler<StripeWebhookCommand, Result>
{
    public async Task<Result> Handle(StripeWebhookCommand request, CancellationToken ct)
    {
        var endpointSecret = config["Stripe:WebhookSecret"];

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                request.Payload,
                request.Signature,
                endpointSecret
            );
        }
        catch (Exception ex)
        {
            return Result.Failure($"Webhook error: {ex.Message}");
        }

        var payload = JsonNode.Parse(request.Payload)!;

        // retrieve payment by provider id where possible
        CampusEats.Persistence.Entities.Payment? payment = null;
        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;

                await payments.UpdateStatusAsync(
                    providerPaymentId: intent.Id,
                    status: "succeeded",
                    transactionId: intent.LatestChargeId
                );

                payment = await payments.GetByProviderIdAsync(intent.Id);
                if (payment == null)
                {
                    logger.LogError("Payment record missing for PaymentIntent {Id}", intent.Id);
                    break;
                }
                
                var order = payment.Order;
                if (order == null)
                    order = await orders.GetBasicOrderAsync(payment.OrderId, ct);
                if (order == null)
                {
                    logger.LogError("Order not found for PaymentIntent {Id}", intent.Id);
                    break;
                }

                var previousStatus = order.Status;

                foreach (var item in order.OrderItems)
                {
                    await menuRepo.DecreaseStockAsync(item.MenuItemId, item.Quantity, ct);
                }

                order.Status = "paid";
                order.UpdatedAt = DateTime.UtcNow;

                // Apply loyalty points only if order wasn't already marked as paid
                if (!string.Equals(previousStatus, "paid", StringComparison.OrdinalIgnoreCase))
                {
                    var md = intent.Metadata ?? new Dictionary<string,string>();
                    int pointsRedeemed = 0;
                    int pointsEarned = 0;
                    if (md.TryGetValue("points_redeemed", out var pr)) int.TryParse(pr, out pointsRedeemed);
                    if (md.TryGetValue("points_earned", out var pe)) int.TryParse(pe, out pointsEarned);

                    if (pointsRedeemed != 0 || pointsEarned != 0)
                    {
                        // Commit reserved points if they were reserved earlier
                        await students.CommitReservedPointsAsync(order.StudentId, pointsRedeemed, pointsEarned);
                    }

                    // Update order fields so the UI can read Original Total / Points used / Earned / Final paid
                    order.LoyaltyPointsUsed = pointsRedeemed;
                    order.TotalLoyaltyPointsEarned = pointsEarned;
                    var paidAmountCents = intent.Amount;
                    decimal paidAmount = Math.Round(paidAmountCents / 100m, 2);
                    order.FinalAmountPaid = paidAmount;
                }

                await uow.SaveChangesAsync();

                await eventsRepo.AddEventAsync(
                    providerPaymentId: intent.Id,
                    eventType: "payment_intent.succeeded",
                    payload: payload,
                    status: "success"
                );

                logger.LogInformation("Payment succeeded & stock updated for Order {OrderId}", order.OrderId);

                break;
            }

            case "payment_intent.payment_failed":
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;

                await payments.UpdateStatusAsync(
                    providerPaymentId: intent.Id,
                    status: "failed"
                );

                payment = await payments.GetByProviderIdAsync(intent.Id);
                // If points were reserved, release them
                if (payment != null)
                {
                    var mdFail = intent.Metadata ?? new Dictionary<string,string>();
                    if (mdFail.TryGetValue("points_redeemed", out var prFail) && int.TryParse(prFail, out var prVal) && prVal > 0)
                    {
                        await students.ReleaseReservedPointsAsync(payment.Order.StudentId, prVal);
                    }
                }

                await eventsRepo.AddEventAsync(
                    providerPaymentId: intent.Id,
                    eventType: "payment_intent.payment_failed",
                    payload: payload,
                    status: "failed"
                );

                logger.LogWarning("Payment failed for intent {Id}", intent.Id);

                break;
            }
        }

        return Result.Success();
    }
}