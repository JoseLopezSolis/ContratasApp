using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Helpers;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;
public partial class ClientsPageViewModel : BasePageViewModel
{
    #region Services
    readonly IClientService _clientService;
    readonly IContractService _contractService;
    readonly INavigationService _navigationService;
    #endregion

    #region Observable properties
        
    [ObservableProperty] 
    ObservableCollection<Client> clients = new();

    [ObservableProperty] private ObservableCollection<Client> filteredClients = new();
        
    [ObservableProperty] string searchText;
        
    [ObservableProperty] bool isRefreshing;
        
    [ObservableProperty] private bool showArchived;


    #endregion

    #region Constructor

    public ClientsPageViewModel(
        IClientService clientService,
        IContractService contractService,
        INavigationService navigationService)
        : base(navigationService)
    {
        _clientService = clientService;
        _navigationService = navigationService;
        _contractService = contractService;
        showArchived = false;
    }

    #endregion

    #region OnChanged & Lifecycle

    public string ArchiveButtonText 
        => ShowArchived ? "Archived" : "Active";
        
    partial void OnSearchTextChanged(string value)
        => SearchCommand.Execute(null);
        
    public override void OnAppearing()
    {
        if (needRefreshPage)
        {
            base.OnAppearing();
            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
            needRefreshPage = false;
        }
    }

    public override async Task InitializeAsync(object navigationData)
    {
        await RefreshAsync();
    }

    #endregion

    #region Relay Commands 

    [RelayCommand]
    async Task ToggleArchivedAsync()
    {
        ShowArchived = !ShowArchived;
        OnPropertyChanged(nameof(ArchiveButtonText));
        await RefreshAsync();
    }
        
    [RelayCommand]
    async Task RefreshAsync()
    {
        if (IsRefreshing) return;      // <- evita dobles ejecuciones
        IsRefreshing = true;
        try
        {
            var list = ShowArchived
                ? await _clientService.GetArchivedAsync()
                : await _clientService.GetAllAsync();

            Clients.Clear();
            foreach (var c in list.OrderByDescending(x => x.Id))
            {
                var contratos = await _contractService
                    .GetByClientIdAsync(c.Id);

                // limpias y agregas a la colección existente
                c.Contracts.Clear();
                foreach (var contrato in contratos
                             .OrderByDescending(x => x.StartDate))
                    c.Contracts.Add(contrato);

                Clients.Add(c);
            }

            // refrescas tu lista filtrada…
            FilteredClients.Clear();
            foreach (var c in Clients)
                FilteredClients.Add(c);
        }
        finally
        {
            IsRefreshing = false;
        }
    }
        
    [RelayCommand]
    async Task Search()
    {
        var query = SearchText?.Trim().ToLower() ?? "";

        FilteredClients.Clear();

        var resultados = string.IsNullOrWhiteSpace(query)
            ? Clients
            : Clients.Where(c =>
                c.Name.ToLower().Contains(query));

        foreach (var c in resultados)
            FilteredClients.Add(c);
    }
        
    [RelayCommand]
    async Task AddClient()
    {
        needRefreshPage = true;
        await NavigationService.GoToAsync(RouteConstants.AddClientPageRoute);
    }
        
        [RelayCommand]
        async Task NavigateToDetailAsync(Client client)
        {
            await Shell.Current.GoToAsync(
                $"{RouteConstants.ClientPageRoute}?clientId={client.Id}");
        }
        
        [RelayCommand]
        async Task DeleteClientAsync(Client client)
        {
            bool answer = await Application.Current.MainPage
                .DisplayAlert("Remove Client",
                    $"Are you sure you want to remove {client.Name}?",
                    "Yes", "No");
            if (!answer)
                return;

            await _clientService.DeleteAsync(client);
            await RefreshAsync();
        }
        
        [RelayCommand]
        async Task UnarchiveClientAsync(Client client)
        {
            bool ok = await Application.Current?.MainPage
                .DisplayAlert("Unarchive",
                    $"Do you want to Unarchive {client.Name}?",
                    "Yes", "No");
            if (!ok) return;

            client.IsArchived = false;
            await _clientService.UpdateAsync(client);
            await RefreshAsync(); // Reload the list 
        }
        
          [RelayCommand]
                async Task EditClientAsync(Client client)
                {
                    needRefreshPage = true;
                    await NavigationService.GoToAsync(
                        $"{RouteConstants.AddClientPageRoute}?clientId={client.Id}");
                }
                
                [RelayCommand]
                async Task ArchiveClientAsync(Client client)
                {
                    if (client == null) return;
        
                    bool ok = await Application.Current.MainPage
                        .DisplayAlert("Archivar",
                            $"¿Archivar a {client.Name}?",
                            "Sí", "No");
                    if (!ok) return;
        
                    await _clientService.ArchiveAsync(client);
                    await RefreshAsync();
                }
        
        #endregion
    }

