using CommunityToolkit.Mvvm.Messaging;
using ToDoUi.Messengers;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.ViewModels;

public class LogOutViewModel(IAuthenticationService _authService)
{
    public bool IsAuthenticated { get; set; }
    public async Task RemoveLocalUserAsync()
    {
        bool userExist =  await _authService.LogOutAsync();
        if(!userExist)
        {
            IsAuthenticated = false;
            WeakReferenceMessenger.Default.Send<LogoutSignalMessage>(new(IsAuthenticated));
        }
        else
        {
            IsAuthenticated = true;
        }
    }
}
