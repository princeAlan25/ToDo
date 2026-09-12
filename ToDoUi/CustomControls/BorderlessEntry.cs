using Microsoft.Maui.Handlers;
namespace ToDoUi.CustomControls;

public partial class BorderlessEntry : Entry
{
    public BorderlessEntry()
    {
        EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if(view is  BorderlessEntry)
            {
#if IOS || MACCATALYST
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }
        });
    }
}
