using CommunityToolkit.Mvvm.ComponentModel;
using MauiIcons.Core;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoUi.Factories;

namespace ToDoUi.Models;

public partial class FlyoutItemModel : BaseIconModel
{
    [ObservableProperty]
    public partial string Title { get; set; }
    [ObservableProperty]
    public partial string Route { get; set; }
    [ObservableProperty]
    public partial bool IsActive { get; set; }
}
