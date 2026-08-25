using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ToDoShared.DTOs;
using ToDoUi.Messengers;
using ToDoUi.Models;
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
    public ObservableCollection<FlyoutItemModel> FlyoutItems { get; set; } = [
        new FlyoutItemModel(){
            Icon = "WbSunny",
            IconColor = Colors.RoyalBlue,
            Title = "My Day",
            Route = "Myday"
        },
        new FlyoutItemModel(){
            Icon = "Star",
            IconColor = Colors.Pink,
            Title = "Important",
            Route = "Important"
        },
        new FlyoutItemModel(){
            Icon = "ViewWeek",
            IconColor = Colors.Green,
            Title = "Planned",
            Route = "Planned"
        },
        new FlyoutItemModel(){
            Icon = "AssignmentInd",
            IconColor= Colors.DarkOliveGreen,
            Title = "Assigned to me",
            Route = "Assigned"
        },
        new FlyoutItemModel(){
            Icon = "EventNote",
            IconColor = Colors.DarkBlue,
            Title = "Tasks",
            Route = "Tasks"
        }];
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

    [RelayCommand]
    public void SetFlyoutItemState(FlyoutItemModel flyoutItem)
    {
        foreach(FlyoutItemModel currentItem in FlyoutItems)
        {
            if(currentItem.Title == flyoutItem.Title)
            {
                currentItem.IsActive = true;
                WeakReferenceMessenger.Default.Send<ActiveFlyoutItemMessage>(new(currentItem));
            }
            else
            {
                currentItem.IsActive = false;
            }
        }
    }
}
