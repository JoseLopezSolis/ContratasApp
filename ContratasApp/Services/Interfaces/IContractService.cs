using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface IContractService
{
    // 1) Crea un LoanContract y genera automáticamente sus PaymentSchedule
    Task<int> CreateAsync(LoanContract contract);

    // 2) Obtiene todos los contratos de un cliente
    Task<List<LoanContract>> GetByClientIdAsync(int clientId);

    // 3) Obtiene el detalle de pagos de un contrato
    Task<List<PaymentSchedule>> GetPaymentSchedulesAsync(int contractId);

    // 4) Marca una cuota como pagada (y, si es la última, cierra el contrato)
    Task MarkPaymentAsPaidAsync(PaymentSchedule payment);

    // 5) Cierra un contrato manualmente
    Task CloseContractAsync(int contractId);
    //Get contract by id
    Task<LoanContract> GetByIdAsync(int contractId);
    Task AddPaymentAsync(PaymentSchedule pago);
}