using ToDoUi.Helpers;

namespace ToDoUi
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            AppShellHelper.RegisterRoutes();
        }
    }
}
