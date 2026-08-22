using Microsoft.Extensions.Logging;
using MauiIcons.Cupertino;
using MauiIcons.Core;
using ToDoUi.Helpers;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm;

namespace ToDoUi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiIconsCore(option =>
                {
                    option.SetDefaultIconSize(16);
                })
                .UseCupertinoMauiIcons()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.RegisterViewModels();
            builder.Services.RegisterViews();
            builder.Services.RegisterExtraComponents();

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
