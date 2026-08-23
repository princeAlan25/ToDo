using ToDoUi.Views;

namespace ToDoUi.Helpers;

public static class AppShellHelper
{
    public static void RegisterRoutes()
    {
        Routing.RegisterRoute($"{nameof(LoginPage)}", typeof(LoginPage));
        Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
        Routing.RegisterRoute(nameof(LogOutPage), typeof(LogOutPage));
    }
}
