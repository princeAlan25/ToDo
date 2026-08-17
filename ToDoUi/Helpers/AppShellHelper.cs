using ToDoUi.Views;

namespace ToDoUi.Helpers;

public static class AppShellHelper
{
    public static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
    }
}
