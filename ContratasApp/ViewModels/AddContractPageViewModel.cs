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
    readonly IContractService _contractService;

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
        IContractService contractService,
        INavigationService navigationService)
        : base(navigationService)
    {
        _contractService = contractService;
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
        await _contractService.CreateAsync(contract);
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
