using ToDoUi.Views;

namespace ToDoUi.Helpers;

public static class AppShellHelper
{
    public static void RegisterRoutes()
    {
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("home", typeof(HomePage));
    }
}
