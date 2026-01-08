using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace CampusEats.Persistence.Repositories;

public class PaymentEventRepository(CampusEatsDbContext db)
{
    public async Task AddEventAsync(
        string providerPaymentId,
        string eventType,
        string? status = null,
        string? message = null)
    {
        var payment = await db.Payments
            .FirstOrDefaultAsync(p => p.ProviderPaymentId == providerPaymentId);

        if (payment == null)
            return;

        var evt = new PaymentEvent
        {
            PaymentId = payment.PaymentId,
            EventType = eventType,
            EventStatus = status ?? "info",
            Message = message,
            CreatedAt = DateTime.UtcNow
        };

        db.PaymentEvents.Add(evt);
        await db.SaveChangesAsync();
    }

    public async Task AddEventAsync(
        string providerPaymentId,
        string eventType,
        JsonNode payload,
        string? status = null,
        string? message = null)
    {
        var payment = await db.Payments
            .FirstOrDefaultAsync(p => p.ProviderPaymentId == providerPaymentId);

        if (payment == null)
            return;

        var evt = new PaymentEvent
        {
            PaymentId = payment.PaymentId,
            EventType = eventType,
            EventStatus = status ?? "info",
            Message = message,

            Payload = payload.ToJsonString(),

            CreatedAt = DateTime.UtcNow
        };

        db.PaymentEvents.Add(evt);
        await db.SaveChangesAsync();
    }
}