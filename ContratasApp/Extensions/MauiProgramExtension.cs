using CommunityToolkit.Maui;
using ContratasApp.Helpers;
using ContratasApp.Models;
using ContratasApp.Services.Implementations;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels;
using ContratasApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using SQLite;

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
        builder.Services.AddTransient<AddContractPageViewModel>();
        builder.Services.AddTransient<ContractDetailPageViewModel>();
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
        builder.Services.AddTransientWithShellRoute<AddContractPage, AddContractPageViewModel>(RouteConstants.AddContractRoute);
        builder.Services.AddTransientWithShellRoute<ClientPage, ClientPageViewModel>(RouteConstants.ClientPageRoute);
        builder.Services.AddTransientWithShellRoute<ContractDetailPage, ContractDetailPageViewModel>(RouteConstants.ContractDetailRoute);

        return builder;
    }
    
    /// <summary>
    /// Register all services
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        
        // only one SQLiteAsyncConnection in the container
        builder.Services.AddSingleton<SQLiteAsyncConnection>(sp =>
        {
          var path = Path.Combine(FileSystem.AppDataDirectory, "app.db3");
          var db   = new SQLiteAsyncConnection(path);
          db.CreateTableAsync<Client>().Wait();
          db.CreateTableAsync<LoanContract>().Wait();
          db.CreateTableAsync<PaymentSchedule>().Wait();
          return db;
        });
        
        // now register both services that depend on it
        builder.Services.AddSingleton<IClientService, ClientService>();
        builder.Services.AddSingleton<IContractService, ContractService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
        return builder;
    }
}