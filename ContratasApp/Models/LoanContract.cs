using ContratasApp.Enums;
using SQLite;

namespace ContratasApp.Models;

public class LoanContract
{
    [PrimaryKey, AutoIncrement]
    public int      Id         { get; set; }

    public int      ClientId   { get; set; }
    public decimal  Principal  { get; set; }
    public LoanType Type       { get; set; }
    public DateTime StartDate  { get; set; }
    public DateTime CreatedAt  { get; set; } = DateTime.Now;
    public bool     IsClosed   { get; set; }
}