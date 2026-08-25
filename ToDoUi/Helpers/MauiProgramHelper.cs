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
        services.AddTransient<LogOutPage>();
        services.AddTransient<SignUpPage>();
        return services;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LogOutViewModel>();
        services.AddTransient<SignUpViewModel>();
        services.AddTransient<ShellViewModel>();
        services.AddTransient<TasksViewModel>();
        return services;
    }

    public static IServiceCollection RegisterExtraComponents(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<AuthHandler>();
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7092");
        }).AddHttpMessageHandler<AuthHandler>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IUserService, UserService>();

        return services;
    }
}
