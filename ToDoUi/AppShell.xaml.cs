using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using ToDoUi.CustomControls;
using ToDoUi.Extensions;
using ToDoUi.Helpers;
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
            if(_viewModel != null)
            {
                await _viewModel.GetAuthenticatedUserAsync();
                if(!_viewModel.IsAuthorized)
                {
                    await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
                }
                else
                {
                    AccountStatus.BindingContext = _viewModel;
                }
            }
        });
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is ShellViewModel viewModel)
        {
            await viewModel.GetAuthenticatedUserAsync();
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
                _viewModel.SetCategoryFocusState(categoryIdParam, true);
            }
        }
    }

    private void OnEntryUnfocused(object? sender, FocusEventArgs e)
    {
        if(sender is  Entry categoryEntry)
        {
            var categoryId = categoryEntry.GetValue(ElementExtensions.ChildIdentityProperty);
            _viewModel.SetCategoryFocusState((int)categoryId, false);
        }
    }

    private async void CategoryIcon_Tapped(object sender, EventArgs e)
    {
        if(sender is ImageButton categoryIconButton)
        {
            if(categoryIconButton.Parent.Parent.Parent is Grid categoryParentGrid)
            {
                await ShowCategoryPopup(categoryIconButton, categoryParentGrid);
            }
        }
    }

    private async Task ShowCategoryPopup(View iconsContainer, View containerParent)
    {
        Popup iconsPopup = new IconsPopup()
        {
            Title = "Category Icons",
            IconsSource = _viewModel.GetAllMaterialIcons()
        };
        await this.ShowPopupAsync(iconsPopup);
    }
}