using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using ToDoUi.Messengers;
using ToDoUi.ViewModels;

namespace ToDoUi.Views;

public partial class LoginPage : ContentPage
{
    private readonly ShellViewModel _shellViewModel;
	public LoginPage(LoginViewModel viewModel, ShellViewModel shellViewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        _shellViewModel = shellViewModel;
        viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }
    private async void GoToSignUp(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignUpPage), true);
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName == "IsAuthorized" && 
            sender is LoginViewModel viewModel && 
            viewModel.IsAuthorized)
        {
            await Shell.Current.GoToAsync("///MyDay", true);
        }
    }
}