using Microsoft.Extensions.Logging;
using MauiIcons.Cupertino;
using MauiIcons.Core;
using ToDoUi.Helpers;

namespace ToDoUi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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

            builder.Services.RegisterExtraComponents();
            builder.Services.RegisterViewModels();
            builder.Services.RegisterViews();


#if DEBUG
    		builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
