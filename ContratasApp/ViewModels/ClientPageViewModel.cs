
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Helpers;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
    public partial class ClientPageViewModel : BasePageViewModel
    {
        readonly IClientService   _clientService;
        readonly IContractService _contractService;

        [ObservableProperty]
        private int clientId;

        [ObservableProperty]
        private Client client;

        [ObservableProperty]
        private ObservableCollection<LoanContract> contracts = new();

        public ClientPageViewModel(
            IClientService clientService,
            IContractService contractService,
            INavigationService navigationService)
            : base(navigationService)
        {
            _clientService   = clientService;
            _contractService = contractService;
        }

        // Triggered by Shell when clientId is passed in query
        partial void OnClientIdChanged(int id)
            => LoadClientAndContractsAsync(id);

        async void LoadClientAndContractsAsync(int id)
        {
            // 1) Load the Client record
            Client = await _clientService.GetByIdAsync(id);
            if (Client == null)
                return;

            // 2) Load associated contracts
            var list = await _contractService.GetByClientIdAsync(id);
            Contracts.Clear();
            foreach (var c in list.OrderByDescending(x => x.StartDate))
                Contracts.Add(c);
        }

        /// <summary>
        /// Manually refresh the contracts list (e.g. pull-to-refresh).
        /// </summary>
        [RelayCommand]
        async Task RefreshContractsAsync()
        {
            if (Client == null)
                return;

            var list = await _contractService.GetByClientIdAsync(Client.Id);
            Contracts.Clear();
            foreach (var c in list.OrderByDescending(x => x.StartDate))
                Contracts.Add(c);
        }

        /// <summary>
        /// Navigate to AddContractPage to create a new loan for this client.
        /// </summary>
        [RelayCommand]
        async Task CreateContractAsync(Client client)
        {
            if (client == null)
                return;

            await NavigationService.GoToAsync(
                $"{RouteConstants.AddContractRoute}?clientId={client.Id}");
        }

        /// <summary>
        /// Navigate to the detail page of the selected contract.
        /// </summary>
        [RelayCommand]
        async Task NavigateToContractDetailAsync(LoanContract contract)
        {
            if (contract == null)
                return;

            await NavigationService.GoToAsync(
                $"contractdetail?contractId={contract.Id}");
        }
    }