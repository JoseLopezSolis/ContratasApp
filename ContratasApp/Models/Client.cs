using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace ContratasApp.Models;
/// <summary>
/// Represents the Client of my application
/// </summary>
public class Client : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }
    public string LastName { get; set; }

    [Ignore]
    public string FullName => $"{Name} {LastName}".Trim();

    public string Phone { get; set; }
    public string Email { get; set; }
    public string PaymentMethod { get; set; }
    public string ImagePath { get; set; }

    public bool IsArchived { get; set; }

    // Colección en memoria de préstamos asociados
    [Ignore]
    public ObservableCollection<LoanContract> Contracts { get; } = new();

    // Conteo rápido de préstamos para mostrar en UI
    
    [Ignore]
    public int ContractsCount => Contracts.Count;
}