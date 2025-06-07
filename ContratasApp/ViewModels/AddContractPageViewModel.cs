using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Enums;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
public partial class AddContractPageViewModel : BasePageViewModel
{
    readonly ILoanService loanService;

    [ObservableProperty]
    private int clientId;

    [ObservableProperty]
    private decimal amount;

    [ObservableProperty]
    private LoanType selectedLoanType;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    public IList<LoanType> LoanTypes { get; } =
        new List<LoanType> { LoanType.Weekly, LoanType.Monthly };

    public AddContractPageViewModel(
        ILoanService loanService,
        INavigationService navigationService)
        : base(navigationService)
    {
        this.loanService = loanService;
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    async Task SaveAsync()
    {
        var contract = new Loan()
        {
            ClientId  = ClientId,
            Amount = Amount,
            Type      = SelectedLoanType,
            StartDate = StartDate,
            IsClosed  = false
        };
        needRefreshPage = true; //To force refresh of the page
        await loanService.CreateAsync(contract);
        await NavigationService.GoBackAsync();
    }

    bool CanSave()
        => Amount > 0
           && Enum.IsDefined(typeof(LoanType), SelectedLoanType)
           && ClientId > 0;

    partial void OnAmountChanged(decimal value)
        => SaveCommand.NotifyCanExecuteChanged();

    partial void OnSelectedLoanTypeChanged(LoanType value)
        => SaveCommand.NotifyCanExecuteChanged();

    partial void OnStartDateChanged(DateTime value)
        => SaveCommand.NotifyCanExecuteChanged();
}
