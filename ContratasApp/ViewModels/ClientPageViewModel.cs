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
        readonly ILoanService _loanService;
        readonly IPaymentService   _paymentService;

        [ObservableProperty]
        private int clientId;

        [ObservableProperty]
        private Client client;

        [ObservableProperty]
        private ObservableCollection<Loan> loans = new();
        
        [ObservableProperty]
        private List<Payment> payments;
        
        [ObservableProperty]
        private Loan selectedContract;
        
        public ClientPageViewModel(
            IClientService clientService,
            IPaymentService paymentService,
            ILoanService loanService,
            INavigationService navigationService)
            : base(navigationService)
        {
            _clientService   = clientService;
            _paymentService   = paymentService;
            _loanService = loanService;
        }

        partial void OnClientIdChanged(int id)
            => LoadClientAndContractsAsync(id);

        async void LoadClientAndContractsAsync(int id)
        {
            Client = await _clientService.GetByIdAsync(id);
    
            var list = await _loanService.GetByClientIdAsync(id);

            Loans.Clear();
    
            foreach (var loan in list.OrderByDescending(x => x.StartDate))
            {
                loan.Payments = await _paymentService.GetPaymentsByLoanIdAsync(loan.Id);
                Loans.Add(loan);
            }
        }

        [RelayCommand]
        async Task RefreshContractsAsync()
        {
            if (Client == null)
                return;

            var list = await _loanService.GetByClientIdAsync(Client.Id);

            Loans.Clear();

            foreach (var loan in list.OrderByDescending(x => x.StartDate))
            {
                loan.Payments = await _paymentService.GetPaymentsByLoanIdAsync(loan.Id);
                Loans.Add(loan);
            }
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
            NavigateToLoanSchedulerCommand.Execute(loan);
        }

        /// <summary>
        /// Navigates to the detail page of the selected contract.
        /// </summary>
        [RelayCommand]
        async Task NavigateToLoanSchedulerAsync(Loan loan)
            => await NavigationService.GoToAsync($"{RouteConstants.LoanSchedulerRoute}?loanId={loan.Id}");
       
        
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
        private async Task SendWhatsappAsync(Client client)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(client.Phone))
                {
                    await Shell.Current.DisplayAlert("Error",
                        "Phone number is missing.", "OK");
                    return;
                }

                var message = $"Hola {client.Name}, te recuerdo que aún está pendiente el pago de la contrata. Te agradeceríamos mucho que pudieras realizarlo pronto. ¡Gracias por tu atención!";
                var encodedMessage = Uri.EscapeDataString(message);
                var personalWhatsApp = $"https://wa.me/{client.Phone}/?text={encodedMessage}";

                if (await Launcher.Default.CanOpenAsync(personalWhatsApp))
                {
                    bool confirm = await Shell.Current.DisplayAlert(
                        "Confirmación",
                        $"¿Estás seguro que deseas enviar un recordatorio por WhatsApp a {client.FullName}?",
                        "Sí",
                        "No"
                    );
                    if (confirm)
                    {
                        await Launcher.Default.OpenAsync(personalWhatsApp);
                        return;
                    }

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