using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface IPaymentService 
{
    Task<List<Payment>> GetPaymentsByLoanIdAsync(int loanId);
    Task AddPaymentAsync(Payment payment);
    Task AddPaymentsAsync(IEnumerable<Payment> payments);
    Task UpdatePaymentAsync(Payment payment);
}