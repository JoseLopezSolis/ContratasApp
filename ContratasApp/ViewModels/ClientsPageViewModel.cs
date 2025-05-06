using CommunityToolkit.Mvvm.Input;
using ContratasApp.Helpers;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

public partial class ClientsPageViewModel : BasePageViewModel
{
    public ClientsPageViewModel(INavigationService navigationService) : base(navigationService)
    {
    }
    
    [RelayCommand]
    private static async Task AddClient()
    {
        await Shell.Current.GoToAsync(RouteConstants.AddClientPageRoute);
    }

}
