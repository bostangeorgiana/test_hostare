using Microsoft.OpenApi.Models;

namespace CampusEats.Configuration;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CampusEats API",
                Version = "v1",
                Description = "CampusEats backend API for cafeteria ordering system"
            });
            
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Enter JWT token below",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    jwtSecurityScheme,
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static void UseSwaggerDocumentation(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
            return;

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "CampusEats API";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "CampusEats API v1");
        });
    }
}
