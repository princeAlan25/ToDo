using MauiIcons.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToDoUi.Factories;

public abstract class BaseIconConverter<TEnumIconFactory> : IValueConverter where TEnumIconFactory : struct, Enum
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            if(value is BaseIconModel baseIconModel && !string.IsNullOrWhiteSpace(baseIconModel.Icon))
            {
                if(Enum.TryParse<TEnumIconFactory>(baseIconModel.Icon, true, out TEnumIconFactory parsedEnumIcon))
                {
                    return parsedEnumIcon.ToImageSource(iconColor: baseIconModel.IconColor, iconSize: baseIconModel.IconSize);
                }
            }
            return GetFallbackIcon().ToImageSource(iconColor: Colors.Black, iconSize: 16.0);
        }
        catch(Exception ex)
        {
            throw new InvalidOperationException($"Icon conversion error: {ex.Message}");
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString();
    }
    protected abstract Enum GetFallbackIcon();
}
