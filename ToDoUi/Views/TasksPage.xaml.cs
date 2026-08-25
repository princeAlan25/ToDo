using ToDoUi.BasePages;
using ToDoUi.ViewModels;
using System.ComponentModel;

namespace ToDoUi.Views;

public partial class TasksPage : ContentBasePage
{
	private readonly TasksViewModel _tasksViewModel;
	public TasksPage(TasksViewModel tasksViewModel)
	{	
		InitializeComponent();
		_tasksViewModel = tasksViewModel;
		BindingContext = _tasksViewModel;

        _tasksViewModel.PropertyChanged += TasksViewModel_PropertyChanged;
	}

    private void TasksViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName == "CurrentFlyoutItem" && sender is TasksViewModel tasksViewModel)
		{
			PageTitleLabel.Text = tasksViewModel.CurrentFlyoutItem.Title;
		}
    }
}