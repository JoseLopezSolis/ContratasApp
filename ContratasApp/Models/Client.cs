using System.Collections.ObjectModel;
using SQLite;

namespace ContratasApp.Models;

public class Client : LoanContract
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string PaymentMethod { get; set; }
    public string ImagePath { get; set; }
    public bool IsArchived { get; set; }
    
    // (Opcional) Relación en memoria con sus contratos
    [Ignore]
    public ObservableCollection<LoanContract> Contracts { get; set; } = new ObservableCollection<LoanContract>();

    // Conteo rápido de contratas
    [Ignore]
    public int ContractsCount => Contracts?.Count ?? 0;
}