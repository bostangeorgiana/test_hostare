using CampusEats.Backend.Menu.Services;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Features.Orders;
using CampusEats.Persistence.Repositories;

namespace CampusEats.Configuration;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IOrderRepository, CampusEats.Persistence.Repositories.OrderRepository>();
        services.AddScoped<IRecommendationRepository, RecommendationRepository>();

        services.AddScoped<UserRepository>();
        services.AddScoped<StudentRepository>();
        services.AddScoped<PaymentRepository>();
        services.AddScoped<ReportRepository>();
        services.AddScoped<PaymentEventRepository>();

        
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}