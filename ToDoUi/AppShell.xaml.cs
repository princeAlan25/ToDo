using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Internals;
using System.Collections;
using System.ComponentModel;
using ToDoUi.CustomControls;
using ToDoUi.Extensions;
using ToDoUi.Helpers;
using ToDoUi.Models;
using ToDoUi.ViewModels;
using ToDoUi.Views;

namespace ToDoUi;

public partial class AppShell : Shell
{
    private readonly ShellViewModel _viewModel;
    public AppShell(ShellViewModel viewModel)
    {
        InitializeComponent();
        AppShellHelper.RegisterRoutes();

        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        //session validation at the startup
        Dispatcher.Dispatch(async () =>
        {
            if (_viewModel != null)
            {
                await _viewModel.GetAuthenticatedUserAsync();
                if (!_viewModel.IsAuthorized)
                {
                    await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
                }
                else
                {
                    AccountStatus.BindingContext = _viewModel;
                    await _viewModel.GetAllCategoriesAsync();
                }
            }
        });
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is ShellViewModel viewModel)
        {
            if(e.PropertyName == "IsAuthorized")
            {
                await viewModel.GetAuthenticatedUserAsync();
            }
            if ((e.PropertyName == "IsAuthorized" || e.PropertyName == "CategoryName") && _viewModel.IsAuthorized)
            {
                await _viewModel.GetAllCategoriesAsync();
            }
        }
    }

    protected override async void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);
        string destination = args.Target?.Location.ToString() ?? "";
        if (_viewModel != null && !_viewModel.IsAuthorized)
        {
            if(!destination.Contains(nameof(LoginPage)) &&
               !destination.Contains(nameof(SignUpPage)))
            {
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
            }
        }
    }

    private async void OnSignOutButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LogOutPage));
    }

    private void MenuFlyoutItem_Clicked(object? sender, EventArgs e)
    {
        if(sender is MenuFlyoutItem menuFlyoutItem)
        {
            if(menuFlyoutItem.Text == "Rename" && menuFlyoutItem.CommandParameter is int categoryIdParam)
            {
                if(menuFlyoutItem.Parent.Parent is Border categoryBorder)
                {
                    if(categoryBorder.Content is Grid categoryGrid)
                    {
                        if(categoryGrid.Children.Last() is Entry categoryEntry)
                        {
                            Dispatcher.Dispatch(() => categoryEntry.Focus());
                        }
                    }
                }
                _viewModel?.SetCategoryFocusState(categoryIdParam, true);
            }
        }
    }

    private async void OnEntryUnfocused(object? sender, FocusEventArgs e)
    {
        if(sender is  Entry categoryEntry)
        {
            var categoryId = categoryEntry.GetValue(ElementExtensions.ChildIdentityProperty);
            _viewModel.SetCategoryFocusState((int)categoryId, false);
            var categoryEntryParent = FindParent<Border>(categoryEntry);
            if(categoryEntryParent != null && categoryEntryParent.BindingContext is FlyoutItemModel categoryObj)
            {
                await _viewModel.UpdateCategoryAsync(categoryObj);
            }
        }
    }

    private async void CategoryIcon_Tapped(object? sender, EventArgs e)
    {
        if(sender is ImageButton categoryIconButton)
        {
            if(categoryIconButton.Parent.Parent.Parent != null)
            {
                await ShowCategoryPopup();
            }
        }
    }

    private async Task ShowCategoryPopup()
    {
        Popup iconsPopup = new IconsPopup()
        {
            Title = "Category Icons",
            IconsSource = ShellViewModel.GetAllMaterialIcons()
        };
        if(_viewModel.IsAuthorized)
        {
            Dispatcher.Dispatch(async () =>
            {
                await this.ShowPopupAsync(iconsPopup);
            });
        }
    }

    private async void AddCategoryButton_Clicked(object? sender, EventArgs e)
    {
        if(sender != null)
        {
            bool categoriesReady = await _viewModel.CreateCategoryAsync();
            if(categoriesReady)
            {
                if (categoriesCollectionView.ItemsSource is IList categoriesCollection)
                {
                    categoriesCollectionView.ScrollTo(categoriesCollection.Count - 1);
                }
            }
        }
    }

    static T? FindParent<T>(Element element) where T : Element
    {
        var current = element;
        while(current != null)
        {
            if(current is T target)
            {
                return target;
            }
            current = current.Parent;
        }
        return null;
    }
}