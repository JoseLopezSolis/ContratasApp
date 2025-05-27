using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Enums;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ContractId), "contractId")]
public partial class ContractDetailPageViewModel:BasePageViewModel
{
    readonly IContractService _contractService;

    [ObservableProperty]
    private int contractId;

    [ObservableProperty]
    private LoanContract contract;

    [ObservableProperty]
    private ObservableCollection<PaymentSchedule> payments = new();

    public ContractDetailPageViewModel(
        IContractService contractService,
        INavigationService navigationService)
        : base(navigationService)
    {
        _contractService = contractService;
    }

    // Fired when Shell injects the contractId query parameter
    partial void OnContractIdChanged(int id)
        => LoadContractAsync(id);

    // Load the LoanContract and its payments
    async void LoadContractAsync(int id)
    {
        Contract = await _contractService.GetByIdAsync(id);
        await LoadPaymentsAsync();
    }

    // Retrieve and populate the Payments collection
    async Task LoadPaymentsAsync()
    {
        var list = await _contractService.GetPaymentSchedulesAsync(Contract.Id);

        // Actualiza el contador de pagos ya hechos
        Contract.PaidCount = list.Count(p => p.IsPaid);

        Payments.Clear();
        int num = 1;
        foreach (var p in list.OrderBy(p => p.DueDate ?? DateTime.MinValue))
        {
            p.PaymentNumber = num++;
            Payments.Add(p);
        }
    }
    
    // Comando para marcar el siguiente pago pendiente como pagado
    [RelayCommand]
    async Task AddPaymentAsync()
    {
        if (Contract.Type == LoanType.Weekly)
        {
            // 1) Busca la siguiente cuota no pagada
            var next = Payments.FirstOrDefault(p => !p.IsPaid);
            if (next != null)
            {
                // 2) Márquela como pagada
                await _contractService.MarkPaymentAsPaidAsync(next);
            }
        }
        else // MonthlyInterest
        {
            // Cada pago es un 10% del principal, se inserta uno nuevo
            var pago = new PaymentSchedule
            {
                ContractId = Contract.Id,
                Amount     = Math.Round(Contract.Principal * 0.10m, 2),
                IsPaid     = true,
                PaidDate   = DateTime.Now
            };
            await _contractService.AddPaymentAsync(pago);
        }

        // 3) Recarga la lista y notifica progreso
        await LoadPaymentsAsync();
        OnPropertyChanged(nameof(Contract.PaymentProgress));
        OnPropertyChanged(nameof(Contract.RemainingPayments));
    }
    
    // Command to mark a payment as paid and refresh the list
    [RelayCommand]
    async Task MarkPaymentAsPaidAsync(PaymentSchedule payment)
    {
        if (payment == null || payment.IsPaid) return;
        await _contractService.MarkPaymentAsPaidAsync(payment);
        await LoadPaymentsAsync();
    }

    // Command to simply close the page
    [RelayCommand]
    async Task CloseAsync()
    {
        await NavigationService.GoBackAsync();
    }
}