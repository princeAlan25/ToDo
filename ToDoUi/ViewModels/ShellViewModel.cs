using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiIcons.Material.Outlined;
using System.Collections.ObjectModel;
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
            CategoryId = 100,
            Icon = "WbSunny",
            IconColor = Colors.RoyalBlue,
            Title = "My Day",
            Route = "Myday"
        },
        new FlyoutItemModel(){
            CategoryId = 200,
            Icon = "Star",
            IconColor = Colors.Pink,
            Title = "Important",
            Route = "Important"
        },
        new FlyoutItemModel(){
            CategoryId = 300,
            Icon = "ViewWeek",
            IconColor = Colors.Green,
            Title = "Planned",
            Route = "Planned"
        },
        new FlyoutItemModel(){
            CategoryId = 400,
            Icon = "AssignmentInd",
            IconColor= Colors.DarkOliveGreen,
            Title = "Assigned to me",
            Route = "Assigned"
        },
        new FlyoutItemModel(){
            CategoryId = 500,
            Icon = "EventNote",
            IconColor = Colors.DarkBlue,
            Title = "Tasks",
            Route = "Tasks"
        }
    ];

    public ObservableCollection<FlyoutItemModel> Categories { get; set; } = [
        new()
        {
            CategoryId = 600,
            Icon = "Category",
            IconColor = Colors.Black,
            Title = "New Category",
            Route = ""
        }
    ];

    [ObservableProperty]
    public partial string UserName { get; set; } = "Username";
    [ObservableProperty]
    public partial string? Email { get; set; } = "example@gmail.com";
    [ObservableProperty]
    public partial bool IsAuthorized { get; set; } = false;

    [ObservableProperty]
    public partial string CategoryName { get; set; } = "Unitled";
    [ObservableProperty]
    public partial string CategoryDescription { get; set; } = "Category description";
    [ObservableProperty]
    public partial string CategoryColorCode { get; set; } = "12,23,45";

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
        if(flyoutItem != null)
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
                    WeakReferenceMessenger.Default.Send<ActiveFlyoutItemMessage>(new(item));
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
                    IconColor = Color.FromRgba(_randomColorCode.Next(50,200), _randomColorCode.Next(50, 200), _randomColorCode.Next(100, 200), 255),
                    Title = category.Name,
                    Route = "",
                    CategoryId = category.CategoryId
                };
                Categories.Add(categoryItem);
            }
        }
    }   

    public static ObservableCollection<string> GetAllMaterialIcons()
    {
        return Enum.GetValues<MaterialOutlinedIcons>()
            .Select(icon => icon.ToString())
            .ToObservableCollection<string>();
    }

    public async Task<bool> CreateCategoryAsync()
    {
        if(!string.IsNullOrWhiteSpace(CategoryName))
        {
            CreateCategoryDto categoryDto = new(CategoryName, CategoryColorCode, CategoryDescription);
            var response = await _categoryService.CreateCategoryAsync(categoryDto);
            if(response != null)
            {
                FlyoutItemModel categoryResult = new()
                {
                    Icon = "Category",
                    IconColor = Color.FromRgba(_randomColorCode.Next(50, 200), _randomColorCode.Next(50, 200), _randomColorCode.Next(100, 200), 255),
                    Title = response.Name,
                    Route = "",
                    CategoryId = response.CategoryId
                };
                Categories.Add(categoryResult);
                return true;
            }
        }
        return false;
    }

    public async Task UpdateCategoryAsync(FlyoutItemModel category)
    {
        foreach(FlyoutItemModel ct in Categories)
        {
            if(ct.CategoryId == category.CategoryId && ct.Title != category.Title)
            {
                UpdateCategoryDto request = new(category.CategoryId ?? 0, category.Title, category.IconColor.ToString(), "Category Updated");
                var response = await _categoryService.UpdateCategoryAsync(request);
                if (response != null)
                {
                    await GetAllCategoriesAsync();
                }
            }
            else
            {
                continue;
            }
        }
    }
}
