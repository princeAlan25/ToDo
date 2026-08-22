using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ToDoShared.DTOs;
using ToDoUi.Messengers;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;
    public ShellViewModel(IUserService userService, IAuthenticationService authService)
    {
        _userService = userService;
        _authService = authService;
        WeakReferenceMessenger.Default.Register<LoginSignalMessage>(this, (recipient, message) =>
        {
            IsAuthorized = message.Value;
        });
        WeakReferenceMessenger.Default.Register<LogoutSignalMessage>(this, (recipient, message) =>
        {
            IsAuthorized = message.Value;
        });
    }

    [ObservableProperty]
    public partial string UserName { get; set; } = "Username";
    [ObservableProperty]
    public partial string? Email { get; set; } = "example@gmail.com";
    [ObservableProperty]
    public partial bool IsAuthorized { get; set; } = false;

    public async Task<bool> GetAuthenticatedUserAsync()
    {
        UserDto? response = await _userService.GetUserByIdAsync();
        if (response != null)
        {
            UserName = response.Name;
            Email = response.Email;
            IsAuthorized = true;
            return true;
        }
        UserName = "Username";
        Email = "example@gmail.com";
        IsAuthorized = false;
        return false;
    }
}
