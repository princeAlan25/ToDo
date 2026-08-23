using System.ComponentModel;
using ToDoUi.Helpers;
using ToDoUi.ViewModels;
using ToDoUi.Views;

namespace ToDoUi;

public partial class AppShell : Shell
{
    private readonly ShellViewModel _viewModel;

    public AppShell(ShellViewModel viewModel)
    {
        InitializeComponent();
        AppShellHelper.RegisterRoutes();

        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        //session validation at the startup
        Dispatcher.Dispatch(async () =>
        {
            if(_viewModel != null)
            {
                await _viewModel.GetAuthenticatedUserAsync();
                if(!_viewModel.IsAuthorized)
                {
                    await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
                }
                else
                {
                    AccountStatus.BindingContext = _viewModel;
                }
            }
        });
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is ShellViewModel viewModel)
        {
            await viewModel.GetAuthenticatedUserAsync();
        }
    }

    protected override async void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);
        string destination = args.Target?.Location.ToString() ?? "";
        if (_viewModel != null && !_viewModel.IsAuthorized)
        {
            if(!destination.Contains(nameof(LoginPage)) &&
               !destination.Contains(nameof(SignUpPage)))
            {
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
            }
        }
    } 

    private async void OnSignOutButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LogOutPage));
    }
}