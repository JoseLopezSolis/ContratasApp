using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface ILoanService
{
    Task<int> CreateAsync(Loan loan); // Create Loan
    Task<Loan> GetByIdAsync(int contractId); //Get Loan by id
    Task<List<Loan>> GetByClientIdAsync(int clientId); // GET list of Loan
    Task CloseContractAsync(int contractId); // Mark a loan as payed
}