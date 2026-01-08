using CampusEats.Features.Orders;
using CampusEats.Persistence.Repositories;
using CampusEats.Shared;
using MediatR;
using Stripe;
using CampusEats.Features.Menu.Interfaces;

namespace CampusEats.Features.Payment.Process;

public class ProcessPaymentHandler(
    IOrderRepository orders,
    PaymentRepository payments,
    StudentRepository students,
    IMenuRepository menuRepo,
    IUnitOfWork uow,
    IConfiguration config,
    ILogger<ProcessPaymentHandler> logger)
    : IRequestHandler<ProcessPaymentCommand, Result<ProcessPaymentResponse>>
{
    public async Task<Result<ProcessPaymentResponse>> Handle(
        ProcessPaymentCommand request,
        CancellationToken ct)
    {
        var order = await orders.GetBasicOrderAsync(request.OrderId, ct);
        if (order == null)
            return Result<ProcessPaymentResponse>.Failure("Order not found.");

        // Must be pending
        if (order.Status != "pending")
            return Result<ProcessPaymentResponse>.Failure(
                $"Order is currently '{order.Status}'. Payment is not allowed."
            );

        var latest = await payments.GetLatestForOrderAsync(order.OrderId);
        if (latest != null && latest.Status == "succeeded")
            return Result<ProcessPaymentResponse>.Failure("Order is already fully paid.");

        if (order.TotalAmount <= 0)
            return Result<ProcessPaymentResponse>.Failure("Order amount is invalid.");

        // Loyalty calculations
        var pointsEarned = (int)Math.Floor(order.TotalAmount * 0.1m);

        // Get student and available points
        var student = await students.GetByIdAsync(order.StudentId);
        var availablePoints = student?.LoyaltyPoints ?? 0;

        var pointsRequested = Math.Max(0, request.PointsToRedeem);
        // Don't allow redeeming more points than order total (1 point == 1 unit)
        var maxRedeemableByOrder = (int)Math.Floor(order.TotalAmount);
        var pointsToApply = Math.Min(pointsRequested, Math.Min(availablePoints, maxRedeemableByOrder));

        // Try to reserve points atomically before proceeding. This prevents race conditions.
        if (pointsToApply > 0)
        {
            var reserved = await students.ReservePointsAsync(order.StudentId, pointsToApply);
            if (!reserved)
            {
                // Refresh available points and recompute a smaller reservation
                var freshStudent = await students.GetByIdAsync(order.StudentId);
                var freshAvailable = freshStudent?.LoyaltyPoints - freshStudent?.ReservedLoyaltyPoints ?? 0;
                var newPointsToApply = Math.Min(pointsRequested, Math.Min(Math.Max(0, freshAvailable), maxRedeemableByOrder));
                if (newPointsToApply > 0)
                {
                    var reserved2 = await students.ReservePointsAsync(order.StudentId, newPointsToApply);
                    if (reserved2)
                        pointsToApply = newPointsToApply;
                    else
                        pointsToApply = 0;
                }
                else
                {
                    pointsToApply = 0;
                }
            }
        }

        // Special case: if points cover the whole order after reservation, commit immediately and finish without Stripe
        if (pointsToApply >= maxRedeemableByOrder && pointsToApply > 0)
        {
            // Deduct reserved and add earned points atomically via repository
            await students.CommitReservedPointsAsync(order.StudentId, pointsToApply, pointsEarned);

            // Mark order as paid and persist using repository helper
            order.Status = "paid";
            order.UpdatedAt = DateTime.UtcNow;

            // Update order fields to reflect points applied and final amount
            order.LoyaltyPointsUsed = pointsToApply;
            order.FinalAmountPaid = 0m;
            order.TotalLoyaltyPointsEarned = pointsEarned;

            await orders.UpdateOrderStatusDirectAsync(order);

            // Decrease stock for each order item
            foreach (var item in order.OrderItems)
            {
                await menuRepo.DecreaseStockAsync(item.MenuItemId, item.Quantity, ct);
            }

            // Ensure changes are flushed
            await uow.SaveChangesAsync();

            var updatedStudent = await students.GetByIdAsync(order.StudentId);

            return Result<ProcessPaymentResponse>.Success(
                new ProcessPaymentResponse(
                    Success: true,
                    Message: "Payment successful using loyalty points.",
                    Amount: 0,
                    AvailablePoints: updatedStudent?.LoyaltyPoints ?? 0,
                    AppliedPoints: pointsToApply,
                    EarnedPoints: pointsEarned
                )
            );
        }

        // Determine amount after applying points (assume 1 point = 1 unit)
        var amountAfterPoints = order.TotalAmount - pointsToApply;
        if (amountAfterPoints < 0) amountAfterPoints = 0;

        StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
        if (string.IsNullOrWhiteSpace(StripeConfiguration.ApiKey))
            return Result<ProcessPaymentResponse>.Failure("Payment configuration error.");
        var payment = await payments.CreatePendingAsync(order.OrderId, order.TotalAmount);

        try
        {
            var intentService = new PaymentIntentService();

            var intentOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)Math.Round(amountAfterPoints * 100m, MidpointRounding.AwayFromZero),
                Currency = "ron",
                PaymentMethod = request.PaymentMethodId,
                Confirm = true,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never"
                },
                Metadata = new Dictionary<string, string>
                {
                    { "payment_id", payment.PaymentId.ToString() },
                    { "order_id", order.OrderId.ToString() },
                    { "points_redeemed", pointsToApply.ToString() },
                    { "points_earned", pointsEarned.ToString() }
                }
            };

            var intent = await intentService.CreateAsync(intentOptions, cancellationToken: ct);

            payment.ProviderPaymentId = intent.Id;
            payment.PaymentIntentSecret = intent.ClientSecret;
            await payments.UpdateAsync(payment);

            return Result<ProcessPaymentResponse>.Success(
                new ProcessPaymentResponse(
                    Success: true,
                    Message: "Payment processing started.",
                    Amount: amountAfterPoints,
                    AvailablePoints: availablePoints,
                    AppliedPoints: pointsToApply,
                    EarnedPoints: pointsEarned
                )
            );
        }
        catch (StripeException sx)
        {
            logger.LogError(sx, "Stripe payment error for order {OrderId}", order.OrderId);
            return Result<ProcessPaymentResponse>.Failure($"Stripe error: {sx.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected payment failure for order {OrderId}", order.OrderId);
            return Result<ProcessPaymentResponse>.Failure("Unexpected error occurred.");
        }
    }
}