using Microsoft.Extensions.Logging;
using Counter.models;
using Counter.views;

namespace Counter
{
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

            builder.Services.AddSingleton<ICounterStore, CounterStore>();

            builder.Services.AddTransient<CountersLogic>();

            
            builder.Services.AddTransient<CountersPage>();
            builder.Services.AddTransient<EditCounterPage>();
           
            return builder.Build();
        }
    }
}
