using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using ToDoShared.DTOs;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.ViewModels;

public partial class LoginViewModel(IAuthenticationService authService) : ObservableValidator
{
    private readonly IAuthenticationService _authService = authService;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [EmailAddress(ErrorMessage = "Invalid Email.")]
    [NotifyPropertyChangedFor(nameof(EmailError), nameof(HasEmailError))]
    private string? email;
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(8, ErrorMessage="Password should be 8 minimum characters long.")]
    [NotifyPropertyChangedFor(nameof(PasswordError), nameof(HasPasswordError))]
    private string? password;

    public string? EmailError => GetErrors(nameof(Email)).FirstOrDefault()?.ErrorMessage;
    public string? PasswordError => GetErrors(nameof(Password)).FirstOrDefault()?.ErrorMessage;

    public bool HasEmailError => !string.IsNullOrWhiteSpace(EmailError);
    public bool HasPasswordError => !string.IsNullOrWhiteSpace(PasswordError);

    partial void OnEmailChanged(string? value)
    {
        ValidateProperty(value, nameof(Email));
        LoginCommand.NotifyCanExecuteChanged();
    }
    partial void OnPasswordChanged(string? value)
    {
        ValidateProperty(value, nameof(Password));
        LoginCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        if(Email != null && Password != null)
        {
            LoginRequestDto request = new(Email, Password);
            loginResponseDto? result = await _authService.LoginAsync(request);
            
        }
    }

    private bool CanLogin() => !HasErrors && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
}
