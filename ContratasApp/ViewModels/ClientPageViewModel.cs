using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Helpers;
using ContratasApp.Models;
using ContratasApp.Services.Implementations;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
    public partial class ClientPageViewModel : BasePageViewModel
    {
        readonly IClientService   _clientService;
        readonly ILoanService _loanService;
        readonly IPaymentService   _paymentService;
        readonly IWhatsappService _whatsappService;

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
            INavigationService navigationService,
            IWhatsappService whatsappService)
            : base(navigationService)
        {
            _clientService   = clientService;
            _paymentService   = paymentService;
            _loanService = loanService;
            _whatsappService = whatsappService;
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
                        "El número de teléfono está vacío.", "OK");
                    return;
                }

                bool confirm = await Shell.Current.DisplayAlert(
                    "Confirmación",
                    $"¿Deseas enviar un recordatorio por WhatsApp a {client.FullName}?",
                    "Sí", "No");

                if (!confirm) return;

                var message = new WhatsappReminderMessage
                {
                    To = client.Phone.StartsWith("52") ? client.Phone : $"52{client.Phone}",
                    TemplateName = "recordatorio_pago",
                    LanguageCode = "es_MX",
                    ClientName = client.Name,
                    Amount = "$500 MXN", // puedes reemplazar con client.Amount si lo tienes
                    LimitDate = "12/05/2025" // o client.DueDate?.ToString("dd/MM/yyyy")
                };

                var service = new WhatsappService(); // clase que crea el HttpClient
                var success = await service.SendReminderAsync(message);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Éxito", "Mensaje enviado por WhatsApp ✅", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "No se pudo enviar el mensaje. Verifica tu backend.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error inesperado: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task ShowAlertAsync()
        {
            await Application.Current.MainPage.DisplayAlert("Test", "message", "OK");
        }
        
    }