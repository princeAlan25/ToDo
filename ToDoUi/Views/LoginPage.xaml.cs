using ToDoUi.ViewModels;

namespace ToDoUi.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
    private async void GoToSignUp(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignUpPage), true);
    }
}