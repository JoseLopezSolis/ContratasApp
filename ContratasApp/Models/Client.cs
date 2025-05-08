using SQLite;

namespace ContratasApp.Models;

public class Client
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string PaymentMethod { get; set; }
    public string ImagePath { get; set; }
    public bool IsArchived { get; set; }
}