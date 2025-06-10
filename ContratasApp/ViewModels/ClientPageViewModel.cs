using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Helpers;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;
using ContratasApp.Views.Popups;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
    public partial class ClientPageViewModel : BasePageViewModel
    {
        readonly IClientService   _clientService;
        readonly ILoanService loanService;

        [ObservableProperty]
        private int clientId;

        [ObservableProperty]
        private Client client;

        [ObservableProperty]
        private ObservableCollection<Loan> loans = new();
        
        [ObservableProperty]
        private Loan selectedContract;
        
        public ClientPageViewModel(
            IClientService clientService,
            ILoanService loanService,
            INavigationService navigationService)
            : base(navigationService)
        {
            _clientService   = clientService;
            this.loanService = loanService;
        }

        partial void OnClientIdChanged(int id)
            => LoadClientAndContractsAsync(id);

        async void LoadClientAndContractsAsync(int id)
        {
            Client = await _clientService.GetByIdAsync(id);
            
            var list = await loanService.GetByClientIdAsync(id);
            Loans.Clear();
            foreach (var loan in list.OrderByDescending(x => x.StartDate))
                Loans.Add(loan);
        }

        [RelayCommand]
        async Task RefreshContractsAsync()
        {
            if (Client == null)
                return;

            var list = await loanService.GetByClientIdAsync(Client.Id);
            Loans.Clear();
            foreach (var client in list.OrderByDescending(x => x.StartDate))
                Loans.Add(client);
        }

        /// <summary>
        /// Navigates to AddContractPage to create a new contract for this client.
        /// </summary>
        
        [RelayCommand]
        async Task CreateContractAsync(Client client)
        {
            if (client == null)
                return;
            needRefreshPage = true;
            await NavigationService.GoToAsync(
                $"{RouteConstants.AddContractRoute}?clientId={client.Id}");
        }
        /// <summary>
        /// Triggered when a contract is selected in the CollectionView.
        /// Executes the navigation command to contract detail.
        /// </summary>
        partial void OnSelectedContractChanged(Loan loan)
        {
            if (loan.Equals(null)) return;
            NavigateToContractDetailCommand.Execute(loan);
        }

        /// <summary>
        /// Navigates to the detail page of the selected contract.
        /// </summary>
        [RelayCommand]
        async Task NavigateToContractDetailAsync(Loan contract)
            => await NavigationService.GoToAsync($"{RouteConstants.ContractDetailRoute}?contractId={contract.Id}");
       
        
        [RelayCommand]
        private async Task CallToClientAsync(string clientNumber)
        {
            try
            {
                var phoneUri = new Uri($"tel:{clientNumber}");
                await Launcher.Default.OpenAsync(phoneUri);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unable to initiate call: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task SendWhatsappAsync(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    await Shell.Current.DisplayAlert("Error", "Phone number is missing.", "OK");
                    return;
                }

                var message = "Que tal, este es un recordatorio para ver si tenias el abono de la contrata.";
                var encodedMessage = Uri.EscapeDataString(message);
                var personalWhatsApp = $"https://wa.me/{phoneNumber}/?text={encodedMessage}";

                if (await Launcher.Default.CanOpenAsync(personalWhatsApp))
                {
                    await Launcher.Default.OpenAsync(personalWhatsApp);
                    return;
                }

                // Neither app is available
                await Shell.Current.DisplayAlert(
                    "WhatsApp Not Available",
                    "Neither WhatsApp nor WhatsApp Business is installed.",
                    "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Unable to send message: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task ShowAlertAsync()
        {
            await Application.Current.MainPage.DisplayAlert("Test", "message", "OK");
        }
        
    }