using SQLite;

namespace ContratasApp.Models;

public class Payment
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }

    [Indexed] public int LoanId { get; set; } // Relación al préstamo

    public DateTime DueDate { get; set; } // Fecha programada del pago

    public decimal Amount { get; set; } // Monto de la cuota (10% del préstamo)

    public bool IsPaid { get; set; } = false; // Estado del pago

    public DateTime? PaidAt { get; set; } // Fecha real de pago, si se pagó

    public string
        Note { get; set; } // Opcional: puedes guardar comentarios (ej: “pago en efectivo”, “pagó tarde”, etc.)
}