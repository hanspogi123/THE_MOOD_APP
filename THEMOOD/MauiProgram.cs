using Microsoft.Extensions.Logging;
using THEMOOD.Services;
using THEMOOD.ViewModels;
using THEMOOD.Pages;

namespace THEMOOD;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<IMoodService, DefaultMoodService>();

        // Register view models
        builder.Services.AddTransient<MoodEntry_VM>();
        builder.Services.AddTransient<CalendarView_VM>();

        // Register pages
        builder.Services.AddTransient<MoodEntryPage>();
        builder.Services.AddTransient<CalendarView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}