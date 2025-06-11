using ContratasApp.Extensions;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace ContratasApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            
            # region Font Awesome
            fonts.AddFont("Brand-Regular-400.otf", "FAB");
            fonts.AddFont("Free-Regular-400.otf", "FAR");
            fonts.AddFont("Free-Solid-900.otf", "FAS");
            #endregion
            
            #region CustomFonts 
            //Intern
            fonts.AddFont("Inter_18pt-Regular.ttf", "InterRegular");
            fonts.AddFont("Inter_18pt-SemiBold.ttf", "InterSemiBold");
            fonts.AddFont("Inter_18pt-SemiBoldItalic.ttf", "InterSemiBoldItalic");
            fonts.AddFont("Inter_18pt-Thin.ttf", "InterThin");
            fonts.AddFont("Inter_18pt-Medium.ttf", "InterMedium");

            
            //RobotoMono 
            fonts.AddFont("RobotoMono-ExtraLight.ttf", "RobotoMonoExtraLight");
            fonts.AddFont("RobotoMono-Regular.ttf", "RobotoMonoRegular");
            fonts.AddFont("RobotoMono-Bold.ttf", "RobotoMonoBold");
            fonts.AddFont("RobotoMono-BoldItalic.ttf", "RobotoMonoBoldItalic");
            
            #endregion
        })
            .RegisterShellRoutes()
            .RegisterViewModels()
            .RegisterServices()
            .UseMauiCommunityToolkit();
        
#if DEBUG
        builder.Logging.AddDebug(); 
#endif
        return builder.Build();
    }
}