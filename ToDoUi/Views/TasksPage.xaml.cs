using ToDoUi.BasePages;

namespace ToDoUi.Views;

public partial class TasksPage : ContentBasePage
{
	public TasksPage()
	{	
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (Shell.Current?.CurrentItem?.CurrentItem?.CurrentItem is ShellContent activeShellContent)
            {
                PageTitleLabel.Text = activeShellContent.Title;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}