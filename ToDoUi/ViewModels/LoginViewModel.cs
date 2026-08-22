using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel.DataAnnotations;
using ToDoShared.DTOs;
using ToDoUi.Messengers;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.ViewModels;

public partial class LoginViewModel : ObservableValidator, IQueryAttributable
{
    private readonly IAuthenticationService _authService;
    public LoginViewModel(IAuthenticationService authService)
    {
        _authService = authService;
        ValidateAllProperties();
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [EmailAddress(ErrorMessage = "Invalid Email.")]
    [NotifyPropertyChangedFor(nameof(EmailError), nameof(HasEmailError))]
    public partial string? Email { get; set; }
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(8, ErrorMessage="Password should be 8 minimum characters long.")]
    [NotifyPropertyChangedFor(nameof(PasswordError), nameof(HasPasswordError))]
    public partial string? Password { get; set; }
    [ObservableProperty]
    public partial bool IsAuthorized { get; set; }

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
            if (result != null)
            {
                IsAuthorized = true;
                WeakReferenceMessenger.Default.Send(new LoginSignalMessage(IsAuthorized));
            }
            else
            {
                IsAuthorized = false;
            }
        }
        else
        {
            IsAuthorized = false;
        }
    }
    private bool CanLogin() => !HasErrors;
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query.TryGetValue("userEmail", out object? userMail))
        {
            if(!string.IsNullOrWhiteSpace((string)userMail)) Email = Uri.UnescapeDataString((string)userMail);
        }
    }
}