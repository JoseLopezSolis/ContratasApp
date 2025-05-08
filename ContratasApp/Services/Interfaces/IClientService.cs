using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface IClientService
{
    Task<List<Client>> GetAllAsync();   
    Task<Client> GetByIdAsync(int id);
    Task<List<Client>> GetArchivedAsync();          
    Task<int> AddAsync(Client client);
    Task<int> UpdateAsync(Client client);
    Task<int> DeleteAsync(Client client);
    Task ArchiveAsync(Client client);
}