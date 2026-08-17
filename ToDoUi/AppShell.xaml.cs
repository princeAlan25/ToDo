using ToDoUi.Helpers;
namespace ToDoUi;
public partial class AppShell : Shell
{
    private bool _isHandlingRootNavigation;
    private HashSet<string> _topLevelRoutes = new();

    public AppShell()
    {
        InitializeComponent();

        // collect top-level routes (FlyoutItems / ShellItems) so we can detect
        // when navigation should go to the root of a flyout item
        foreach (var item in Items)
        {
            if (!string.IsNullOrWhiteSpace(item.Route))
                _topLevelRoutes.Add(item.Route.Trim('/'));
        }

        AppShellHelper.RegisterRoutes();
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        if (_isHandlingRootNavigation)
        {
            base.OnNavigating(args);
            return;
        }

        try
        {
            //current navigation target
            var target = args?.Target?.Location?.ToString() ?? string.Empty;

            //extract all routes hierarchy
            var segments = target.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 0)
            {
                var first = segments[0];
                if (_topLevelRoutes.Contains(first))
                {
                    args?.Cancel();
                    // Hook and Perform the navigation on Main thread
                    _ = MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        try
                        {
                            _isHandlingRootNavigation = true;
                            await Shell.Current.GoToAsync($"//{first}", true);
                        }
                        finally
                        {
                            _isHandlingRootNavigation = false;
                        }
                    });
                    return;
                }
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        base.OnNavigating(args);
    }
}