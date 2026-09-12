using System.Globalization;

namespace ToDoUi.Converters;

public class ActiveFlyoutItemConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isItemActive && isItemActive)
        {
            if (Application.Current != null && Application.Current.Resources.TryGetValue("ActiveFlyoutItemStyle", out var activeItemKeyStyle))
            {
                return activeItemKeyStyle as Style;
            }
        }
        else
        {
            if (Application.Current != null && Application.Current.Resources.TryGetValue("FlyoutItemStyle", out var itemKeyStyle))
            {
                return itemKeyStyle as Style;
            }
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
