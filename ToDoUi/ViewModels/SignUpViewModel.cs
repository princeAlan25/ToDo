using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using ToDoShared.DTOs;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.ViewModels;

public partial class SignUpViewModel(IAuthenticationService authService) : ObservableValidator
{
    private readonly IAuthenticationService _authService = authService;

    [ObservableProperty]
    [EmailAddress(ErrorMessage = "Invalid Email.")]
    [Required(ErrorMessage = "Email field is required.")]
    [NotifyPropertyChangedFor(nameof(EmailError), nameof(HasEmailError))]
    public partial string Email { get; set; }
    [ObservableProperty]
    [MinLength(3, ErrorMessage = "Username should have Three characters long required.")]
    [Required(ErrorMessage = "Username field is required.")]
    [NotifyPropertyChangedFor(nameof(UserNameError), nameof(HasUserNameError))]
    public partial string UserName { get; set; }
    [ObservableProperty]
    [Required(ErrorMessage = "Password field is required.")]
    [MinLength(8, ErrorMessage = "Password should have 8 Characters long.")]
    [NotifyPropertyChangedFor(nameof(PasswordError), nameof(HasPasswordError))]
    public partial string Password { get; set; }
    [ObservableProperty]
    public partial Dictionary<string, object>? LoginPayLoad { get; set; }

    public string? EmailError => GetErrors(nameof(Email)).FirstOrDefault()?.ErrorMessage;
    public string? UserNameError => GetErrors(nameof(UserName)).FirstOrDefault()?.ErrorMessage;
    public string? PasswordError => GetErrors(nameof(Password)).FirstOrDefault()?.ErrorMessage;

    public bool HasEmailError => !string.IsNullOrWhiteSpace(EmailError);
    public bool HasUserNameError => !string.IsNullOrWhiteSpace(UserName);
    public bool HasPasswordError => !string.IsNullOrWhiteSpace(PasswordError);

    partial void OnEmailChanged(string value)
    {
        ValidateProperty(value, nameof(Email));
        SignUpCommand.NotifyCanExecuteChanged();
    }

    partial void OnUserNameChanged(string value)
    {
        ValidateProperty(value, nameof(UserName));
        SignUpCommand.NotifyCanExecuteChanged();
    }

    partial void OnPasswordChanged(string value)
    {
        ValidateProperty(value, nameof(Password));
        SignUpCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanSignUp))]
    private async Task SignUp()
    {
        SignUpRequestDto user = new(Email, UserName, Password);
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
        {
            ValidateAllProperties();
            LoginPayLoad = null;
            return;
        }
        else
        {
            UserDto? response = await _authService.SignUpAsync(user);
            if (response != null)
            {
                LoginPayLoad = new() { { "userEmail", response.Email } };
            }
            return;
        }
    }
    private bool CanSignUp() => !HasErrors;
}
