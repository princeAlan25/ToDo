using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using ToDoEntityModels.Models;
using ToDoShared.DTOs;
using ToDoUi.Messengers;
using ToDoUi.Models;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly Random _randomColorCode = new ();
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;
    private readonly ICategoryService _categoryService;
    public ShellViewModel(IUserService userService, IAuthenticationService authService, ICategoryService categoryService)
    {
        _userService = userService;
        _authService = authService;
        _categoryService = categoryService;
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
            CategoryId = 0,
            Icon = "WbSunny",
            IconColor = Colors.RoyalBlue,
            Title = "My Day",
            Route = "Myday"
        },
        new FlyoutItemModel(){
            CategoryId = 1,
            Icon = "Star",
            IconColor = Colors.Pink,
            Title = "Important",
            Route = "Important"
        },
        new FlyoutItemModel(){
            CategoryId = 2,
            Icon = "ViewWeek",
            IconColor = Colors.Green,
            Title = "Planned",
            Route = "Planned"
        },
        new FlyoutItemModel(){
            CategoryId = 3,
            Icon = "AssignmentInd",
            IconColor= Colors.DarkOliveGreen,
            Title = "Assigned to me",
            Route = "Assigned"
        },
        new FlyoutItemModel(){
            CategoryId = 4,
            Icon = "EventNote",
            IconColor = Colors.DarkBlue,
            Title = "Tasks",
            Route = "Tasks"
        }
    ];

    public ObservableCollection<FlyoutItemModel> Categories { get; set; } = [
        new()
        {
            CategoryId = 5,
            Icon = "Category",
            IconColor = Colors.DarkGray,
            Title = "New Category",
            Route = "",
        }
    ];

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
        int maxLength = Math.Max(FlyoutItems.Count, Categories.Count);
        for (int itemIdx = 0; itemIdx < maxLength; itemIdx++)
        {
            if((itemIdx < FlyoutItems.Count))
            {
                if (FlyoutItems[itemIdx].CategoryId == flyoutItem.CategoryId)
                {
                    if(!FlyoutItems[itemIdx].IsActive)
                    {
                        FlyoutItems[itemIdx].IsActive = true;
                        WeakReferenceMessenger.Default.Send<ActiveFlyoutItemMessage>(new(FlyoutItems[itemIdx]));
                    }
                }
                else
                {
                    if(FlyoutItems[itemIdx].IsActive) FlyoutItems[itemIdx].IsActive = false;
                }
            }
            if((itemIdx < Categories.Count))
            {
                if (Categories[itemIdx].CategoryId == flyoutItem.CategoryId)
                {
                    Categories[itemIdx].IsActive = true;
                    WeakReferenceMessenger.Default.Send<ActiveFlyoutItemMessage>(new(Categories[itemIdx]));
                }
                else
                {
                    if(Categories[itemIdx].IsActive)
                    {
                        Categories[itemIdx].IsActive = false;
                        continue;
                    }
                }
            }
        }
    }

    public async Task GetAllCategoriesAsync()
    {
        var response = await _categoryService.GetCategoriesAsync();
        if(response != null)
        {
            foreach(CategoryDto category in response)
            {
                FlyoutItemModel categoryItem = new()
                {
                    Icon = "Category",
                    IconColor = Color.FromRgba(_randomColorCode.Next(50,200), _randomColorCode.Next(50, 200), _randomColorCode.Next(100, 200), 1),
                    Title = category.Name,
                    Route = "",
                    CategoryId = category.CategoryId
                };
                Categories.Add(categoryItem);
            }
        }
    }
}
