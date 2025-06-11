using ContratasApp.Enums;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using SQLite;

namespace ContratasApp.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly SQLiteAsyncConnection _db;

    public PaymentService(SQLiteAsyncConnection db)
    {
        _db = db;
        _ = _db.CreateTableAsync<Payment>(); 
    }

    public async Task<List<Payment>> GetPaymentsByLoanIdAsync(int loanId)
    {
        return await _db.Table<Payment>()
            .Where(p => p.LoanId == loanId)
            .OrderBy(p => p.DueDate)
            .ToListAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        await _db.InsertAsync(payment);
    }

    public async Task AddPaymentsAsync(IEnumerable<Payment> payments)
    {
        await _db.InsertAllAsync(payments);
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        await _db.UpdateAsync(payment);
    }
}