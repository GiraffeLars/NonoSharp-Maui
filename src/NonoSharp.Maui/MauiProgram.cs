using Microsoft.Extensions.Logging;
using NonoSharp.Maui.Data;
using CommunityToolkit.Maui;

namespace NonoSharp.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Add database as singleton
            builder.Services.AddSingleton<Database>();

            // Add SettingsService. Is initialized after the app has been built.
            builder.Services.AddSingleton<SettingsService>();

            var app = builder.Build();

            SettingsService settingsService = app.Services.GetRequiredService<SettingsService>();
            Task.Run(() => settingsService.InitializeAsync());

            return app;
        }
    }
}
