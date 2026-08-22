using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoUi.Messengers;

public class LogoutSignalMessage(bool value) : ValueChangedMessage<bool>(value) { }
