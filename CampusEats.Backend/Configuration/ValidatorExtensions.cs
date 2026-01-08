using CampusEats.Features.Auth.Login;
using CampusEats.Features.Auth.Register;
using CampusEats.Features.Menu.CreateMenuItem;
using CampusEats.Features.Orders.CreateOrder;
using CampusEats.Features.Payment.Process;
using FluentValidation;

namespace CampusEats.Configuration;

public static class ValidatorExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoginCommand>, LoginUserValidator>();
        services.AddScoped<IValidator<RegisterCommand>, RegisterValidator>();
        services.AddScoped<IValidator<ProcessPaymentCommand>, ProcessPaymentValidator>();
        services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderValidator>();
        services.AddScoped<IValidator<CreateMenuItemCommand>, CreateMenuItemValidator>();


        return services;
    }
}