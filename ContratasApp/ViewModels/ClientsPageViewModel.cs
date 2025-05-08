using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Helpers;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

    public partial class ClientsPageViewModel : BasePageViewModel
    {
        readonly IClientService _clientService;
        
        readonly INavigationService _navigationService;

        [ObservableProperty] ObservableCollection<Client> clients = new();

        [ObservableProperty] private ObservableCollection<Client> filteredClients = new();
        
        [ObservableProperty] string searchText;
        
        [ObservableProperty]
        bool isRefreshing;

        // Nueva bandera que indica si mostramos archivados
        [ObservableProperty] private bool showArchived;
        

        public ClientsPageViewModel(
            IClientService clientService,
            INavigationService navigationService)
            : base(navigationService)
        {
            _clientService = clientService;
            _navigationService = navigationService;
            showArchived = false;
        }
        
        // Texto dinámico para el botón
        public string ArchiveButtonText 
            => ShowArchived ? "Archived" : "Active";
        
        // Comando que alterna bandera y recarga
        [RelayCommand]
        async Task ToggleArchivedAsync()
        {
            ShowArchived = !ShowArchived;
            OnPropertyChanged(nameof(ArchiveButtonText));
            await RefreshAsync();   // <--- aquí recargas según la nueva bandera
        }

        [RelayCommand]
        async Task RefreshAsync()
        {
            // DEBUG: 
            Debug.WriteLine($"RefreshAsync – ShowArchived={ShowArchived}");

            var list = ShowArchived
                ? await _clientService.GetArchivedAsync()
                : await _clientService.GetAllAsync();

            Clients.Clear();
            foreach (var c in list.OrderByDescending(x => x.Id))
                Clients.Add(c);

            FilteredClients.Clear();
            foreach (var c in Clients)
                FilteredClients.Add(c);
        }
        
        [RelayCommand]
        async Task UnarchiveClientAsync(Client client)
        {
            if (client == null) return;

            bool ok = await Application.Current.MainPage
                .DisplayAlert("Desarchivar",
                    $"¿Quieres desarchivar a {client.Name}?",
                    "Sí", "No");
            if (!ok) return;

            client.IsArchived = false;
            await _clientService.UpdateAsync(client);
            await RefreshAsync(); // recarga lista según ShowArchived
        }

        
        [RelayCommand]
        async Task EditClientAsync(Client client)
        {
            if (client == null) return;
            await NavigationService.GoToAsync(
                $"{RouteConstants.AddClientPageRoute}?clientId={client.Id}");
        }
        
        // Nuevo: archivar
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
        
        // Comando que filtra según SearchText
        [RelayCommand]
        void Search()
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
        
         // Cada vez que cambie SearchText, lanzamos automáticamente la búsqueda
         partial void OnSearchTextChanged(string value)
             => SearchCommand.Execute(null);

        public override void OnAppearing()
        {
            base.OnAppearing();
            // Dispara la carga inicial
            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
        }
        
        //Method to go to a new view
        [RelayCommand]
        async Task AddClient()
        {
            await NavigationService.GoToAsync(RouteConstants.AddClientPageRoute);
        }
        
        // Comando que toma un Client y navega a detalle
        [RelayCommand]
        async Task NavigateToDetailAsync(Client client)
        {
            if (client == null) return;

            // Shell URI with query
            await Shell.Current.GoToAsync(
                $"{RouteConstants.ClientPageRoute}?clientId={client.Id}");
        }
    }

