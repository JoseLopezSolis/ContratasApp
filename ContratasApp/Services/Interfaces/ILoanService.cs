using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface ILoanService
{
    Task<int> CreateAsync(Loan loan); //Create a loan for a Client
    Task<List<Loan>> GetByClientIdAsync(int clientId); //Get a list of loans of a Client
    //GET - Detail of payments
    Task<List<PaymentSchedule>> GetPaymentSchedulesAsync(int contractId);

    //MARK - Mark a coute as payed
    Task MarkPaymentAsPaidAsync(PaymentSchedule payment);

    // 5) Cierra un contrato manualmente
    Task CloseContractAsync(int contractId);
    //Get contract by id
    Task<Loan> GetByIdAsync(int contractId);
    Task AddPaymentAsync(PaymentSchedule pago);
}