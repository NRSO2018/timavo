using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Timavo.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Timavo.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
    .AddHttpMessageHandler<Timavo.Client.Services.AnalyticsCacheInvalidationHandler>();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddRadzenComponents();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Timavo.ServerAPI"));

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<Timavo.Client.Services.UndoNotificationService>();
builder.Services.AddScoped<Timavo.Client.Services.ProjectColorService>();
builder.Services.AddScoped<Timavo.Client.Services.ViewportService>();
builder.Services.AddScoped<Timavo.Client.Services.MobileSheetService>();
builder.Services.AddScoped<Timavo.Client.Services.UserProfileService>();
builder.Services.AddScoped<Timavo.Client.Services.ThemeService>();
builder.Services.AddScoped<Timavo.Client.Services.TokenService>();
builder.Services.AddScoped<Timavo.Client.Services.McpActivityService>();
builder.Services.AddSingleton<Timavo.Client.Services.AnalyticsService>();
builder.Services.AddTransient<Timavo.Client.Services.AnalyticsCacheInvalidationHandler>();

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = builder.HostEnvironment.BaseAddress.TrimEnd('/');
    options.ProviderOptions.ClientId = "Timavo.Client";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("Timavo.ServerAPI");
});

await builder.Build().RunAsync();
