using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace ContratasApp.Models;
public class Client : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string PaymentMethod { get; set;} 
    public string ImagePath { get; set; }
    public bool IsArchived { get; set; }

    [Ignore]
    public string FullName => $"{Name} {LastName}".Trim();

    [Ignore]
    public ObservableCollection<Loan> Loans { get; } = new();

    [Ignore]
    public int ContractsCount => Loans.Count;
}