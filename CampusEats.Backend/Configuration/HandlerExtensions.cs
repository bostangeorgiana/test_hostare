using CampusEats.Features.Auth.Login;
using CampusEats.Features.Auth.Register;
using CampusEats.Features.Auth.RefreshToken;
using CampusEats.Features.Auth.Logout;
using CampusEats.Features.HealthChecks;
using CampusEats.Features.Admin.ManageAdmin;
using CampusEats.Features.Admin.ManageAdmin.CreateAdmin;
using CampusEats.Features.Admin.ManageKitchenStaff;
using CampusEats.Features.Admin.ManageStudent;
using CampusEats.Features.Menu.UpdateStock;
using CampusEats.Features.Orders.CreateOrder;
using CampusEats.Features.Payment.Process;
using CampusEats.Features.Payment.Webhooks;

namespace CampusEats.Configuration;

public static class HandlerExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<GetDbHealthHandler>();

        services.AddScoped<LoginHandler>();
        services.AddScoped<RegisterHandler>();
        services.AddScoped<RefreshTokenHandler>();
        services.AddScoped<LogoutHandler>();

        services.AddScoped<CreateAdminHandler>();
        services.AddScoped<DeleteAdminHandler>();
        services.AddScoped<GetAdminsHandler>();
        
        services.AddScoped<CreateKitchenStaffHandler>();
        services.AddScoped<GetKitchenStaffHandler>();
        services.AddScoped<DeleteKitchenStaffHandler>();
        
        services.AddScoped<GetStudentRequestsHandler>();
        services.AddScoped<ApproveStudentRequestHandler>();
        services.AddScoped<RejectStudentRequestHandler>();
        services.AddScoped<GetStudentsAdminHandler>();

        services.AddScoped<UpdateStockHandler>();
        
        services.AddScoped<CreateOrderHandler>();
        
        services.AddScoped<StripeWebhookHandler>();
        services.AddScoped<ProcessPaymentHandler>();

        return services;
    }
}
