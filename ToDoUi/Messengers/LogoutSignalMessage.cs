using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ToDoUi.Messengers;

public class LogoutSignalMessage(bool value) : ValueChangedMessage<bool>(value) { }
