using SQLite;

namespace ContratasApp.Models;

public class Payment
{
    [PrimaryKey, AutoIncrement]
    public int     Id          { get; set; }
    public int     LoanId  { get; set; } 
    public decimal Amount      { get; set; }
    public DateTime DueDate    { get; set; }
    public bool    IsPaid      { get; set; }
}