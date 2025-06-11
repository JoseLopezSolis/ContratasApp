using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(LoanId), "loanId")]
public partial class LoanSchedulerPageViewModel : BasePageViewModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILoanService _loanService;

    public LoanSchedulerPageViewModel(
        IPaymentService paymentService,
        ILoanService loanService,
        INavigationService navigationService) : base(navigationService)
    {
        _paymentService = paymentService;
        _loanService = loanService;
    }

    [ObservableProperty]
    private Loan loan;

    [ObservableProperty]
    private ObservableCollection<Payment> payments;

    [ObservableProperty]
    private int loanId;

    partial void OnLoanIdChanged(int value)
    {
        LoadLoanAndPaymentsAsync(value);
    }

    private async void LoadLoanAndPaymentsAsync(int loanId)
    {
        Loan = await _loanService.GetByIdAsync(loanId);

        var result = await _paymentService.GetPaymentsByLoanIdAsync(loanId);

        if (Payments == null)
            Payments = new ObservableCollection<Payment>();

        Payments.Clear();
        foreach (var payment in result)
            Payments.Add(payment);
    }

    [RelayCommand]
    private async Task TogglePaymentAsync(Payment payment)
    {
        payment.IsPaid = !payment.IsPaid;
        payment.PaidAt = payment.IsPaid ? DateTime.Now : null;
        await _paymentService.UpdatePaymentAsync(payment);
    }
}
