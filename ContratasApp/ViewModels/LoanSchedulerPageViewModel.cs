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
    private readonly IPaymentService paymentService;
    private readonly ILoanService loanService;

    public LoanSchedulerPageViewModel(IPaymentService paymentService,
        ILoanService loanService,
        INavigationService navigationService) : base(navigationService)
    {
        this.paymentService = paymentService;
        this.loanService = loanService;
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
        Loan = await loanService.GetByIdAsync(loanId);
        var result = await paymentService.GetPaymentsByLoanIdAsync(loanId);
        Payments = new ObservableCollection<Payment>(result);
    }

    [RelayCommand]
    private async Task TogglePaymentAsync(Payment payment)
    {
        payment.IsPaid = !payment.IsPaid;
        payment.PaidAt = payment.IsPaid ? DateTime.Now : null;
        await paymentService.UpdatePaymentAsync(payment);
    }
}
