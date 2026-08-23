using ToDoUi.Helpers;
using ToDoUi.ViewModels;

namespace ToDoUi.Views;

public partial class LogOutPage : ContentPage
{
	private readonly LogOutViewModel _logoutViewModel;
	private readonly ShellViewModel _shellViewModel;
	public LogOutPage(LogOutViewModel logoutViewModel, ShellViewModel shellViewModel)
	{
		InitializeComponent();

		_logoutViewModel = logoutViewModel;
		_shellViewModel = shellViewModel;
		BindingContext = logoutViewModel;
		Dispatcher.Dispatch(async () =>
		{
            await _logoutViewModel.RemoveLocalUserAsync();
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        });
	}
}