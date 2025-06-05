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

    [ObservableProperty] 
    private ObservableCollection<Client> filteredClients = new();
        
    [ObservableProperty] 
    string searchText;
        
    [ObservableProperty] 
    bool isRefreshing;
        
    [ObservableProperty] 
    private bool showArchived;
    #endregion
    
    public string EmptyClientsMessage => Clients.Count > 0 
        ? "" 
        : $"No hay clientes {(ShowArchived ? "archivados" : "activos")}";

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
        Clients.CollectionChanged += (s, e) => OnPropertyChanged(nameof(EmptyClientsMessage));
    }

    #endregion

    #region OnChanged & Lifecycle
    
    partial void OnShowArchivedChanged(bool oldValue, bool newValue)
    {
        OnPropertyChanged(nameof(EmptyClientsMessage));
    }

    public string ArchiveButtonText 
        => ShowArchived ? "Archivados" : "Activos";
    
        
    partial void OnSearchTextChanged(string value)
        => SearchCommand.Execute(null);
        
    public override void OnAppearing()
    {
        if (needRefreshPage)
        {
            base.OnAppearing();
            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
            
            needRefreshPage = false; // Flag to avoid unexpected refresh page
        }
    }

    public override async Task InitializeAsync(object navigationData) => await RefreshAsync();

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
        if (IsRefreshing) return; //Avoid refreshing twise     
        IsRefreshing = true; 
        try
        {
            var listClients = ShowArchived
                ? await _clientService.GetArchivedAsync()
                : await _clientService.GetAllAsync();

            Clients.Clear(); 
            
            foreach (var client in listClients.OrderByDescending(client => client.Id))
            {
                List<Loan> loans = await _contractService
                    .GetByClientIdAsync(client.Id);
                client.Loans.Clear();
                foreach (var loan in loans
                             .OrderByDescending(x => x.StartDate))
                    client.Loans.Add(loan);

                Clients.Add(client);
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
            .DisplayAlert("Eliminar cliente",
                $"Estas seguro que deseas eliminar a {client.Name}?",
                "Eliminar", "Cancelar");
        if (!answer)
            return;

        await _clientService.DeleteAsync(client);
        await RefreshAsync();
    }
        
    [RelayCommand]
    async Task UnarchiveClientAsync(Client client)
        {
            bool isClientArchived = await Application.Current?.MainPage
                .DisplayAlert("Desarchivar",
                    $"Quieres desarchivar {client.Name}?",
                    "Si", "Cancelar");
            if (!isClientArchived) return;

            client.IsArchived = false;
            await _clientService.UpdateAsync(client);
            await RefreshAsync(); 
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
                            "Sí", "Cancelar");
                    if (!ok) return;
        
                    await _clientService.ArchiveAsync(client);
                    await RefreshAsync();
                }
        
    #endregion

    #region Extra Methods 
    
    #endregion
}

