
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
        
        [ObservableProperty]
        private LoanContract selectedContract;

        public ClientPageViewModel(
            IClientService clientService,
            IContractService contractService,
            INavigationService navigationService)
            : base(navigationService)
        {
            _clientService   = clientService;
            _contractService = contractService;
        }

        // Called by Shell when the query parameter clientId changes
        partial void OnClientIdChanged(int id)
            => LoadClientAndContractsAsync(id);

        async void LoadClientAndContractsAsync(int id)
        {
            // 1) Load the Client entity
            Client = await _clientService.GetByIdAsync(id);
            if (Client == null)
                return;

            // 2) Load and sort contracts by StartDate descending
            var list = await _contractService.GetByClientIdAsync(id);
            Contracts.Clear();
            foreach (var c in list.OrderByDescending(x => x.StartDate))
                Contracts.Add(c);
        }

        /// <summary>
        /// Refreshes the contract list (e.g. pull-to-refresh)
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
        /// Navigates to AddContractPage to create a new contract for this client.
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
        /// Triggered when a contract is selected in the CollectionView.
        /// Executes the navigation command to contract detail.
        /// </summary>
        partial void OnSelectedContractChanged(LoanContract contract)
        {
            if (contract == null) return;
            NavigateToContractDetailCommand.Execute(contract);
        }

        /// <summary>
        /// Navigates to the detail page of the selected contract.
        /// </summary>
        [RelayCommand]
        async Task NavigateToContractDetailAsync(LoanContract contract)
            => await NavigationService.GoToAsync($"{RouteConstants.ContractDetailRoute}?contractId={contract.Id}");
    }