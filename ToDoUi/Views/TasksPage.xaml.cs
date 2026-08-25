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
	}
}