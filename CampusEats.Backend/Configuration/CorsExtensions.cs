namespace CampusEats.Configuration;

public static class CorsExtensions
{
    private const string DefaultPolicyName = "DefaultCorsPolicy";

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        var origins = config.GetSection("Cors:AllowedOrigins").Get<string[]>();

        if (origins == null || origins.Length == 0)
        {
            origins = new[]
            {
                "https://localhost:5237",
                "http://localhost:5237",
                "http://localhost:5140",
                "https://localhost:5140",
                "http://localhost:7202",
                "https://localhost:7202",
                "http://localhost:7221",
                "https://localhost:7221",
                "http://localhost:5273",
                "https://localhost:5273"
            };
        }

        services.AddCors(options =>
        {
            options.AddPolicy(DefaultPolicyName, builder =>
            {
                builder
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
    {
        return app.UseCors(DefaultPolicyName);
    }
}