using Microsoft.Extensions.Logging;
using THEMOOD.Services;
using THEMOOD.ViewModels;
using THEMOOD.Pages;
using CommunityToolkit.Maui;
using Microcharts.Maui;

namespace THEMOOD;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMicrocharts()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Genty-Sans-Regular.ttf", "GentySans");
            });

        // Register view models
        builder.Services.AddTransient<MoodEntry_VM>();

        // Register pages
        builder.Services.AddTransient<MoodEntryPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}