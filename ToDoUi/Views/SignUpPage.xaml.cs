using ToDoUi.ViewModels;

namespace ToDoUi.Views;

public partial class SignUpPage : ContentPage
{
	private readonly SignUpViewModel _viewModel;
	public SignUpPage(SignUpViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;

        _viewModel.PropertyChanged += _viewModel_PropertyChanged;
	}

    private async void _viewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if(e.PropertyName == "LoginPayLoad" && sender is SignUpViewModel signUpViewModel && signUpViewModel.LoginPayLoad != null)
		{
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true, signUpViewModel.LoginPayLoad);
        }
    }
}