using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace ToDoUi.CustomControls;

public partial class IconsPopup : Popup
{
    private CancellationTokenSource _debounceTokenSource = new();
	public static BindableProperty TitleProperty =
		BindableProperty.Create("Title", typeof(string), typeof(IconsPopup), "Popup Title");
	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

    public static BindableProperty IconsSourceProperty =
    BindableProperty.Create("IconsSource", typeof(ObservableCollection<string>), typeof(IconsPopup), new ObservableCollection<string>());
    public ObservableCollection<string> IconsSource
    {
        get => (ObservableCollection<string>)GetValue(IconsSourceProperty);
        set => SetValue(IconsSourceProperty, value);
    }

    public static BindableProperty ScrolledToBottomProperty =
    BindableProperty.Create("IconsSource", typeof(EventHandler), typeof(IconsPopup), null);
    
    public EventHandler ScrolledToBottom
    {
        get => (EventHandler)GetValue(ScrolledToBottomProperty);
        set => SetValue(ScrolledToBottomProperty, value);
    }

    public IconsPopup()
	{
		InitializeComponent();
		BindingContext = this;
	}

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _debounceTokenSource?.Cancel();
        _debounceTokenSource = new();
        var token = _debounceTokenSource.Token;

        try
        {
            await Task.Delay(300, token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        if (token.IsCancellationRequested)
            return;

        if (sender is SearchBar iconsSearchBar)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (!string.IsNullOrWhiteSpace(e.NewTextValue) && e.NewTextValue != e.OldTextValue)
                {
                    var snapshot = IconsSource.ToList();
                    ObservableCollection<string> searchedIcons = snapshot
                        .Where(icon => icon.StartsWith(e.NewTextValue) || icon.EndsWith(e.NewTextValue))
                        .ToObservableCollection<string>();

                    if (searchedIcons.Count > 0)
                    {
                        iconsCollection.ItemsSource = searchedIcons;
                        return;
                    }

                    iconsCollection.ItemsSource = IconsSource;
                    return;
                }
                iconsCollection.ItemsSource = IconsSource;
                return;
            });
        }
    }
}