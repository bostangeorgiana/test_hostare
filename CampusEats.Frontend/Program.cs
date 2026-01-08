using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CampusEats.Frontend;
using CampusEats.Frontend.Features.Admin.Services;
using CampusEats.Frontend.Features.Kitchen.Services;
using CampusEats.Frontend.Features.Menu.Services;
using CampusEats.Frontend.Features.Payment.Services;
using CampusEats.Frontend.Features.Student.Menu.Favorites;
using CampusEats.Frontend.Services;
using CampusEats.Frontend.Services.Auth;
using CampusEats.Frontend.Shared.Http;
using Microsoft.AspNetCore.Components.Authorization;
using CampusEats.Frontend.Features.Users.Services;
using CampusEats.Frontend.Features.Recommendations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

const string backendUrl = "https://localhost:7228";

builder.Services.AddAuthorizationCore();

// Token service (localStorage)
builder.Services.AddScoped<ITokenService, BrowserTokenService>();

// Custom Authentication Provider
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>()
);

// Auth state service (role, user)
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddTransient<AuthMessageHandler>();

builder.Services.AddScoped(sp =>
    new HttpClient(sp.GetRequiredService<AuthMessageHandler>())
    {
        BaseAddress = new Uri(backendUrl)
    }
);
builder.Services.AddScoped<LoggingHttpClient>();
builder.Services.AddScoped<AdminApi>();
builder.Services.AddScoped<IAuthApi, AuthApi>();
builder.Services.AddScoped<MenuApi>();
builder.Services.AddScoped<OrderApi>();
builder.Services.AddScoped<KitchenOrderApi>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<UserApi>();
builder.Services.AddScoped<FavoritesService>();
builder.Services.AddScoped<RecommendationsApi>();
builder.Services.AddScoped<CampusEats.Frontend.Features.Student.Cart.Services.CartService>();

await builder.Build().RunAsync();
