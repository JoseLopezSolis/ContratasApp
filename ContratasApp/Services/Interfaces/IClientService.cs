using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface IClientService
{
    Task<List<Client>> GetAllActiveClients(); //Get active clients
    Task<List<Client>> GetArchivedAsync(); // Get Archived clients
    Task<Client> GetByIdAsync(int id); //Get specific client
    Task<int> AddAsync(Client client); //Add Client to the List of clients
    Task<int> UpdateAsync(Client client); //Update Client information
    Task<int> DeleteAsync(Client client); //Delete Client of the list 
    Task ArchiveAsync(Client client); //Convert a client archived
}