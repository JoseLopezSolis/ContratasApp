using SQLite;

namespace ContratasApp.Models;

public class Payment
{
    [PrimaryKey, AutoIncrement]
    public int     Id          { get; set; }

    public int     ContractId  { get; set; }      // FK back to LoanContract.Id
    public decimal Amount      { get; set; }
    
    public DateTime DueDate    { get; set; }
    public bool    IsPaid      { get; set; }
}