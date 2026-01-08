using CampusEats.Configuration;
using CampusEats.Features.Loyalty.AddLoyaltyPoints;
using CampusEats.Features.Loyalty.GetLoyaltyPoints;
using CampusEats.Features.Orders;
using CampusEats.Features.Payment.Webhooks;
using CampusEats.Middleware;
using CampusEats.Persistence.Context;
using CampusEats.Persistence.Repositories;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Proxy / Reverse-proxy support (important on cloud)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Services
builder.Services.AddSwaggerDocumentation();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationMediatR();
builder.Services.AddRepositories();
builder.Services.AddValidators();
builder.Services.AddHandlers();

builder.Services.AddStripePayments(builder.Configuration);

// Extra registrations (as you had them)
builder.Services.AddScoped<AddLoyaltyPointsHandler>();
builder.Services.AddScoped<GetLoyaltyPointsHandler>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<IOrderRepository, CampusEats.Persistence.Repositories.OrderRepository>();

var app = builder.Build();

// Forwarded headers MUST be early in the pipeline
app.UseForwardedHeaders();

// DB init (keep it safe for deploy: only in Development)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CampusEatsDbContext>();
    await db.Database.EnsureCreatedAsync();
}

// Stripe webhook should be anonymous
app.MapStripeWebhook().AllowAnonymous();

// Health endpoint (anonymous, easy to test on Railway)
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
   .AllowAnonymous();

// HTTPS redirection only in Development (avoid proxy issues in Production)
if (app.Environment.IsDevelopment())
{
    app.UseWhen(
        ctx => !ctx.Request.Path.StartsWithSegments("/payments/webhook"),
        b => b.UseHttpsRedirection()
    );
}

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseCors("DefaultCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Your feature endpoints
app.MapAppEndpoints();

// Dev-only tools (assuming your extension checks env internally)
app.UseDevelopmentTools();

await app.RunAsync();
