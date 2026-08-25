using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ToDoUi.Messengers;
using ToDoUi.Models;

namespace ToDoUi.ViewModels;

public partial class TasksViewModel : ObservableObject
{
    public TasksViewModel()
    {
        WeakReferenceMessenger.Default.Register<ActiveFlyoutItemMessage>(this, (recipient, message) =>
        {
            if(message.Value != null && message.Value is FlyoutItemModel flyoutItem && flyoutItem != CurrentFlyoutItem)
            {
                CurrentFlyoutItem = flyoutItem;
            }
        });
    }

    [ObservableProperty]
    public partial FlyoutItemModel CurrentFlyoutItem { get; set; } = new()
    {
        Icon = "WbSunny",
        IconColor = Colors.RoyalBlue,
        Title = "My Day",
        Route = "Myday"
    };
}
