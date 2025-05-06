using CommunityToolkit.Maui;
using ContratasApp.Helpers;
using ContratasApp.Services.Implementations;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels;
using ContratasApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace ContratasApp.Extensions;

public static class MauiProgramExtension
{
    /// <summary>
    /// Register all viewmodels
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<AddClientPageViewModel>();
        builder.Services.AddTransient<ClientPageViewModel>();
        builder.Services.AddTransient<ClientsPageViewModel>();
        builder.Services.AddTransient<ConfigurationPageViewModel>();
        return builder;
    }
    
    /// <summary>
    /// Register all shell routes
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder RegisterShellRoutes(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientWithShellRoute<AddClientPage, AddClientPageViewModel>(RouteConstants.AddClientPageRoute);

        return builder;
    }
    
    /// <summary>
    /// Register all services
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
        return builder;
    }
}