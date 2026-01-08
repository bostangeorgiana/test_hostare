using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Persistence.Repositories;

public class PaymentRepository(CampusEatsDbContext db)
{
    public async Task<Payment> CreateOrUpdateAsync(
        int orderId,
        decimal amount,
        string providerPaymentId,
        string clientSecret)
    {
        var existing = await db.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);

        if (existing == null)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                ProviderPaymentId = providerPaymentId,
                PaymentIntentSecret = clientSecret,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            db.Payments.Add(payment);
            await db.SaveChangesAsync();
            return payment;
        }

        existing.Amount = amount;
        existing.ProviderPaymentId = providerPaymentId;
        existing.PaymentIntentSecret = clientSecret;

        await db.SaveChangesAsync();
        return existing;
    }
    
    public async Task<Payment?> GetByProviderIdAsync(string providerPaymentId)
    {
        return await db.Payments
            .Include(p => p.Order)
                .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(p => p.ProviderPaymentId == providerPaymentId);
    }


    public async Task UpdateStatusAsync(
        string providerPaymentId,
        string status,
        string? transactionId = null)
    {
        var payment = await db.Payments
            .FirstOrDefaultAsync(p => p.ProviderPaymentId == providerPaymentId);

        if (payment == null)
            return;

        payment.Status = status;
        payment.TransactionId = transactionId;

        await db.SaveChangesAsync();
    }
    
    public async Task<Payment> CreatePendingAsync(int orderId, decimal amount)
    {
        var existing = await db.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);

        if (existing != null)
        {
            existing.Amount = amount;
            existing.Status = "pending";
            existing.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return existing;
        }

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = amount,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        db.Payments.Add(payment);
        await db.SaveChangesAsync();
        return payment;
    }
    
    public async Task UpdateAsync(Payment payment)
    {
        payment.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
    
    public async Task<Payment?> GetLatestForOrderAsync(int orderId)
    {
        return await db.Payments
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();
    }
}