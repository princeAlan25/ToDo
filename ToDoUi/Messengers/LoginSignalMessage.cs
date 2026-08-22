using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ToDoUi.Messengers;
public class LoginSignalMessage(bool value) : ValueChangedMessage<bool>(value){}
