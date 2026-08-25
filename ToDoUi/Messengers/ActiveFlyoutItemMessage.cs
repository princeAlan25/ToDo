using CommunityToolkit.Mvvm.Messaging.Messages;
using ToDoUi.Models;

namespace ToDoUi.Messengers
{
    public class ActiveFlyoutItemMessage(FlyoutItemModel value) : ValueChangedMessage<FlyoutItemModel>(value) { }
}
