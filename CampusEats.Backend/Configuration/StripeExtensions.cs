using Stripe;

namespace CampusEats.Configuration;

public static class StripeExtensions
{
    public static IServiceCollection AddStripePayments(
        this IServiceCollection services, IConfiguration config)
    {
        StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
        return services;
    }
}