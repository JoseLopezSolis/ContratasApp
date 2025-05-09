using ContratasApp.Enums;
using SQLite;

namespace ContratasApp.Models;

public class LoanContract
{
    [PrimaryKey, AutoIncrement]
    public int      Id          { get; set; }
    public int      ClientId    { get; set; }     // FK a Cliente
    public decimal  Principal   { get; set; }     // Monto prestado
    public LoanType Type        { get; set; }     // Weekly o MonthlyInterest
    public DateTime StartDate   { get; set; }     // Fecha de inicio
    public bool     IsClosed    { get; set; }     // True si ya se completó
}