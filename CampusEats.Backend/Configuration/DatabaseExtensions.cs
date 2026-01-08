using CampusEats.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Configuration;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services, 
        IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Connection string 'Default' is missing. Use: " +
                "dotnet user-secrets set \"ConnectionStrings:Default\" \"Host=...\"");

        services.AddNpgsqlDataSource(connectionString);

        services.AddDbContextPool<CampusEatsDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            
            options.EnableSensitiveDataLogging(config.GetValue<bool>("Database:EnableSensitiveLogging"));
        });

        return services;
    }
}