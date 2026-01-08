using CampusEats.Configuration;
using CampusEats.Features.Payment.Webhooks;
using CampusEats.Middleware;
using CampusEats.Persistence.Context;
using CampusEats.Features.Loyalty.AddLoyaltyPoints; 
using CampusEats.Features.Loyalty.GetLoyaltyPoints;
using CampusEats.Persistence.Repositories; 
using CampusEats.Features.Orders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerDocumentation();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationMediatR();
builder.Services.AddRepositories();
builder.Services.AddValidators();
builder.Services.AddHandlers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddStripePayments(builder.Configuration);
builder.Services.AddScoped<AddLoyaltyPointsHandler>();
builder.Services.AddScoped<GetLoyaltyPointsHandler>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<IOrderRepository, CampusEats.Persistence.Repositories.OrderRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CampusEatsDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapStripeWebhook().AllowAnonymous();

app.UseWhen(
    ctx => !ctx.Request.Path.StartsWithSegments("/payments/webhook"),
    appBuilder => appBuilder.UseHttpsRedirection()
);

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseCors("DefaultCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapAppEndpoints();

app.UseDevelopmentTools();

await app.RunAsync();