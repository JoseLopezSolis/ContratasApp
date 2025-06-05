using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface IContractService
{
    //CREATE - Loan contract and generate the payment schedule.
    Task<int> CreateAsync(Loan contract);
    
    //GET - List of loan contract giving clientId.
    Task<List<Loan>> GetByClientIdAsync(int clientId);

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