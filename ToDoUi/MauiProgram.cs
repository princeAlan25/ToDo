using Microsoft.Extensions.Logging;
using MauiIcons.Material;
using ToDoUi.Helpers;
using CommunityToolkit.Maui;
using MauiIcons.Core;
using MauiIcons.Material.Outlined;

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
                .UseMauiIconsCore(iconFactory =>
                {
                    iconFactory.SetDefaultIconSize(16.0);
                    iconFactory.SetDefaultIconAutoScaling(true);
                })
                .UseMaterialMauiIcons()
                .UseMaterialOutlinedMauiIcons()
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
