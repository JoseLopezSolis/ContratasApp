using ContratasApp.Enums;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using SQLite;

namespace ContratasApp.Services.Implementations;

public class LoanService : ILoanService
{
    readonly SQLiteAsyncConnection _db;
    
    public LoanService(SQLiteAsyncConnection db)
    {
        _db = db;
        _db.CreateTableAsync<Loan>().Wait();
    }
    
    //CREATE - Loan 
    public async Task<int> CreateAsync(Loan loan)
    {
        var id = await _db.InsertAsync(loan);
        return id;
    }
    
    //GET - Loan by id 
    public Task<Loan> GetByIdAsync(int contractId) => 
        _db.FindAsync<Loan>(contractId);
    
    //GET - list of Loans
    public async Task<List<Loan>> GetByClientIdAsync(int clientId)
    {
        return await _db.Table<Loan>()
            .Where(client => client.ClientId == clientId)
            .OrderByDescending(client => client.CreatedAt)
            .ToListAsync();
    }
    
    //SET - Loan as closed
    public async Task CloseContractAsync(int contractId)
    {
        var contrato = await _db.FindAsync<Loan>(contractId);
        if (contrato == null) return;
        contrato.IsClosed = true;
        await _db.UpdateAsync(contrato);
    }


}