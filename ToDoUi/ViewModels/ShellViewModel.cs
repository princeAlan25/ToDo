using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiIcons.Material;
using MauiIcons.Material.Outlined;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            IconColor = Colors.Black,
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
    [ObservableProperty]
    public partial bool ActivateIconsSelector { get; set; }

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
        bool isNewActivated = false;
        foreach (FlyoutItemModel item in FlyoutItems.Concat(Categories))
        {
            if (item.CategoryId == flyoutItem.CategoryId)
            {
                if (item.IsActive) return;
                item.IsActive = true;
                isNewActivated = true;
                WeakReferenceMessenger.Default.Send<ActiveFlyoutItemMessage>(new(item));
            }
            else
            {
                if (item.IsActive && isNewActivated)
                {
                    item.IsActive = false;
                    return;
                }
                else if (item.IsActive && !isNewActivated)
                {
                    item.IsActive = false;
                }
            }
        }
    }

    public void ToggleIconsSelector()
    {
        ActivateIconsSelector = !ActivateIconsSelector;
    }

    public void SetCategoryFocusState(int categoryId, bool inFocusMode)
    {
        if(inFocusMode)
        {
            bool isModificationModeSet = false;
            foreach (FlyoutItemModel item in Categories)
            {
                if (item.CategoryId == categoryId)
                {
                    item.InModificationMode = true;
                    isModificationModeSet = true;
                    continue;
                }
                else
                {
                    if (item.InModificationMode && isModificationModeSet)
                    {
                        item.InModificationMode = false;
                        return;
                    }
                    else if (item.InModificationMode && !isModificationModeSet)
                    {
                        item.InModificationMode = false;
                    }
                }
            }
            return;
        }
        else
        {
            foreach(FlyoutItemModel item in Categories)
            {
                if(item.CategoryId == categoryId)
                {
                    item.InModificationMode = false;
                    return;
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

    public ObservableCollection<string> GetAllMaterialIcons()
    {
        return Enum.GetValues<MaterialOutlinedIcons>()
            .Select(icon => icon.ToString())
            .ToObservableCollection<string>();
    }
}
