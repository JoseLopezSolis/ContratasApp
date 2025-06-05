using ContratasApp.Enums;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using SQLite;

namespace ContratasApp.Services.Implementations;

public class ContractService : IContractService
{
    readonly SQLiteAsyncConnection _db;
    public ContractService(SQLiteAsyncConnection db)
    {
        _db = db;
        _db.CreateTableAsync<Loan>().Wait();
        _db.CreateTableAsync<PaymentSchedule>().Wait();
    }
    
    //CREATE - Loan Contract
    public async Task<int> CreateAsync(Loan contract)
    {
        var id = await _db.InsertAsync(contract);
        var schedules = new List<PaymentSchedule>();
        
        if (contract.Type == LoanType.Weekly)
        {
            var cuota = contract.Amount * 0.10m;
            for (int i = 1; i <= 13; i++)
                schedules.Add(new PaymentSchedule {
                    ContractId = id,
                    DueDate    = contract.StartDate.AddDays(7 * i),
                    Amount     = cuota,
                    IsPaid     = false
                });
        }
        
        if (contract.Type == LoanType.Monthly)
        {
            var interes = contract.Amount * 0.10m;
            // Dejanos generar 12 meses de interés
            for (int m = 1; m <= 12; m++)
                schedules.Add(new PaymentSchedule {
                    ContractId = id,
                    DueDate    = contract.StartDate.AddMonths(m),
                    Amount     = interes,
                    IsPaid     = false
                });
        }
        contract.CreatedAt = DateTime.Now;
        await _db.InsertAllAsync(schedules);
        return id;
    }
    
    //GET - List of loan contracts
    public async Task<List<Loan>> GetByClientIdAsync(int clientId)
    {
        return await _db.Table<Loan>()
            .Where(c => c.ClientId == clientId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    //GET - Contract by id 
    public Task<Loan> GetByIdAsync(int contractId)
    {
        return _db.FindAsync<Loan>(contractId);
    }

    
    //SET - Contract as closed
    public async Task CloseContractAsync(int contractId)
    {
        var contrato = await _db.FindAsync<Loan>(contractId);
        if (contrato == null) return;
        contrato.IsClosed = true;
        await _db.UpdateAsync(contrato);
    }
    
    //UPDATE - payment to paid
    public async Task MarkPaymentAsPaidAsync(PaymentSchedule payment)
    {
        payment.IsPaid   = true;
        payment.PaidDate = DateTime.Now;
        await _db.UpdateAsync(payment);
    }

    //ADD - Payment to loan
    public async Task AddPaymentAsync(PaymentSchedule payment)
    {
        payment.IsPaid   = true;
        payment.PaidDate = DateTime.Now;
        await _db.InsertAsync(payment);
    }

    //GET - List of schedules payments
    public Task<List<PaymentSchedule>> GetPaymentSchedulesAsync(int contractId)
        => _db.Table<PaymentSchedule>()
            .Where(p => p.ContractId == contractId)
            .ToListAsync();
}