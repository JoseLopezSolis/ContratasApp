using SQLite;

namespace ContratasApp.Models;

public class PaymentSchedule
{
    [PrimaryKey, AutoIncrement]
    public int      Id          { get; set; }
    public int      ContractId  { get; set; }     // FK a LoanContract
    public DateTime DueDate     { get; set; }     // Fecha de vencimiento
    public decimal  Amount      { get; set; }     // Importe a pagar
    public bool     IsPaid      { get; set; }     // Estado de pago
    public DateTime? PaidDate   { get; set; }     // Nuevo: fecha y hora en que realmente se pagó
    [Ignore]
    public int      PaymentNumber { get; set; }     // Ignorado por SQLite, lo calcularemos en el VM
}