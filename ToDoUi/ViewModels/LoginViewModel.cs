using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoUi.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
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

    partial void OnEmailChanged(string? value) => ValidateProperty(value, nameof(Email));
    partial void OnPasswordChanged(string? value) => ValidateProperty(value, nameof(Password));

    [RelayCommand]
    private void Login()
    {
        if (HasErrors) return;
    }
}
