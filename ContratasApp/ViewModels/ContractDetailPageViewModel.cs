using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        // Asigna el número de pago (1, 2, 3, …) y refresca la lista
        Payments.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].PaymentNumber = i + 1;
            Payments.Add(list[i]);
        }
    }
    
    // Comando para marcar el siguiente pago pendiente como pagado
    [RelayCommand]
    async Task AddPaymentAsync()
    {
        var next = Payments.FirstOrDefault(p => !p.IsPaid);
        if (next == null) return;

        await _contractService.MarkPaymentAsPaidAsync(next);
        await LoadPaymentsAsync();
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