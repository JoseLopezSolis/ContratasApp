
using CommunityToolkit.Mvvm.ComponentModel;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
public partial class ClientPageViewModel : BasePageViewModel
{
    readonly IClientService _clientService;

    // Backing field for the query parameter
    [ObservableProperty]
    int clientId;

    // The loaded Client
    [ObservableProperty]
    Client client;

    public ClientPageViewModel(
        IClientService clientService,
        INavigationService navigationService)
        : base(navigationService)
    {
        _clientService = clientService;
    }

    // When Shell sets ClientId, load the client
    partial void OnClientIdChanged(int id)
        => LoadClient(id);

    async void LoadClient(int id)
    {
        Client = await _clientService.GetByIdAsync(id);
    }
}
