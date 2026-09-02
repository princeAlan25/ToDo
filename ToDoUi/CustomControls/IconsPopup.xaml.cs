using CommunityToolkit.Maui.Views;
using MauiIcons.Material.Outlined;
using System.Collections.ObjectModel;

namespace ToDoUi.CustomControls;

public partial class IconsPopup : Popup
{
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

    public IconsPopup()
	{
		InitializeComponent();
		BindingContext = this;
	}
}