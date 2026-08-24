using ToDoUi.Services.Interfaces;
using ToDoUi.ViewModels;
using ToDoUi.Views;

namespace ToDoUi;

public partial class App : Application
{
    private Color _bgThemeMode = Color.FromArgb("#FFFBF8FF");
    private Color _fgThemeMode = Color.FromArgb("#FF1A1B21");
    private readonly string _icon = "apptitleicon.png";
    private Window _window;
    private readonly AppShell _shell;
    public App(AppShell shell)
    {
        InitializeComponent();
        _shell = shell;
        _window = new Window(_shell);
        Application.Current?.RequestedThemeChanged += App_RequestedThemeChanged;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        //initial theme binding
        _window.TitleBar = new TitleBar
        {
            Title = "Todo Sphere",
            Icon = _icon,
            BackgroundColor = Application.Current?.RequestedTheme == AppTheme.Light
            ? Color.FromArgb("#FFFBF8FF")
            : Color.FromArgb("#FF1A1B21"),
            ForegroundColor = Application.Current?.RequestedTheme == AppTheme.Light
            ? Color.FromArgb("#FF1A1B21")
            : Color.FromArgb("#FFE3E1E9")
        };
        return _window;
    }

    private void App_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        _bgThemeMode = Application.Current?.RequestedTheme == AppTheme.Light
            ? Color.FromArgb("#FFFBF8FF")
            : Color.FromArgb("#FF1A1B21");
        _fgThemeMode = Application.Current?.RequestedTheme == AppTheme.Light
            ? Color.FromArgb("#FF1A1B21")
            : Color.FromArgb("#FFE3E1E9");
        _window.TitleBar = new TitleBar
        {
            Title = "Todo Sphere",
            Icon = _icon,
            BackgroundColor = _bgThemeMode,
            ForegroundColor = _fgThemeMode
        };
    }
}