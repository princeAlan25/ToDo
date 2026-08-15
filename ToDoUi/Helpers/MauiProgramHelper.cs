using ToDoUi.Networking.Handlers;
using ToDoUi.Networking.Implementations;
using ToDoUi.Networking.Interfaces;
using ToDoUi.Services.Implementations;
using ToDoUi.Services.Interfaces;
using ToDoUi.ViewModels;
using ToDoUi.Views;

namespace ToDoUi.Helpers;

public static class MauiProgramHelper
{
    public static IServiceCollection RegisterViews(this IServiceCollection services)
    {
        services.AddTransient<LoginPage>();
        return services;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        return services;
    }

    public static IServiceCollection RegisterExtraComponents(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<AuthHandler>();
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7092");
        }).AddHttpMessageHandler<AuthHandler>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<AppShell>();

        return services;
    }
}
