using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace ContratasApp.Models;

public partial class PaymentSchedule : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int ContractId { get; set; }

    // Fecha programada de vencimiento (sólo para pagos semanales)
    public DateTime? DueDate { get; set; }

    // Monto a pagar en esta cuota
    public decimal Amount { get; set; }

    // Marca si ya fue pagado
    public bool IsPaid { get; set; }

    // Fecha y hora real del pago
    public DateTime? PaidDate { get; set; }

    // Número de pago (1..N), asignado en el ViewModel
    [Ignore]
    public int PaymentNumber { get; set; }

    // Texto legible del estado del pago
    [Ignore]
    public string DisplayStatus =>
        IsPaid
            ? $"✓ Pagado ({PaidDate:dd/MM/yyyy HH:mm})"
            : "Pendiente";
}