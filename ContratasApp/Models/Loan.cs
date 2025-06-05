using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContratasApp.Enums;
using SQLite;

namespace ContratasApp.Models;

public class Loan : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int ClientId { get; set; }
    public decimal Amount { get; set; }
    public LoanType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsClosed { get; set; }

    // Monto de cada cuota semanal (10% del Amount)
    [Ignore]
    public decimal WeeklyPaymentAmount => Math.Round(Amount * 0.10m, 2);

    // Número total de pagos según el tipo
    [Ignore]
    public int TotalPayments => Type == LoanType.Weekly ? 13 : 0;

    // Cuotas ya abonadas (se calcula en el ViewModel)
    [Ignore]
    public int PaidCount { get; set; }

    // Cuotas restantes
    [Ignore]
    public int RemainingPayments => TotalPayments > 0 ? TotalPayments - PaidCount : 0;

    // Progreso en porcentaje para usar en ProgressBar
    [Ignore]
    public double PaymentProgress => TotalPayments > 0 ? (double)PaidCount / TotalPayments : 0;

    // Estado legible
    [Ignore]
    public string Status => IsClosed ? "Cerrado" : "Activo";
}