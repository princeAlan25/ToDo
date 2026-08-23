using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoUi.Factories;

public abstract partial class BaseIconModel : ObservableObject
{
    [ObservableProperty]
    public partial string Icon { get; set; }
    [ObservableProperty]
    public partial Color IconColor { get; set; } = Colors.Black;
    [ObservableProperty]
    public partial double IconSize { get; set; } = 16.0;
}
