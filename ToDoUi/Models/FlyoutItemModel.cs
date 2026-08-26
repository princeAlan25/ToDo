using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ToDoShared.DTOs;
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
    [ObservableProperty]
    public partial bool InModificationMode { get; set; }
    [ObservableProperty]
    public partial int? CategoryId { get; set; }
}
