using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContratasApp.Enums;
using SQLite;

namespace ContratasApp.Models;

public partial class Loan : ObservableObject
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
    
    [Ignore]
    public string LoanTypeTranslated => Type switch
    {
        LoanType.Weekly => "Semanal",
        LoanType.Monthly => "Mensual",
        _ => "Desconocido"
    };
        
    [Ignore]
    public List<Payment> Payments { get; set; } = new();

    [Ignore]
    public int PaidPayments => Payments?.Count(p => p.IsPaid) ?? 0;

    [Ignore]
    public int RemainingPayments => Payments?.Count(p => !p.IsPaid) ?? 0;
    
    [Ignore]
    public string LoanStatus => RemainingPayments == 0 ? "Pagado ✅" : "Activo";

    [Ignore]
    public int TotalPayments => Payments?.Count ?? 0;

    [Ignore]
    public double Progress
    {
        get
        {
            int totalExpectedPayments = Type switch
            {
                LoanType.Weekly => 13,
                LoanType.Monthly => 10,
                _ => 1
            };

            double rawProgress = (double)PaidPayments / totalExpectedPayments;
            return Math.Min(rawProgress, 1.0);
        }
    }


}