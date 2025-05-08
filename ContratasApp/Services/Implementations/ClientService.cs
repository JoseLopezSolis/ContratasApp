using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using SQLite;

namespace ContratasApp.Services.Implementations;

public class ClientService: IClientService
{
    private readonly SQLiteAsyncConnection _database;

    public ClientService(string dbpath)
    {
        //Initialize the connection to SQLite in the path given
        _database = new SQLiteAsyncConnection(dbpath);
        
        //Create the table Client 
        _database.CreateTableAsync<Client>().Wait();
    }

    /// <summary>
    /// Add new client
    /// </summary>
    public Task<int> AddAsync(Client client) =>
        _database.InsertAsync(client);
    
    /// <summary>
    /// Get client by id
    /// </summary>
    public Task<Client> GetByIdAsync(int id) =>
        _database.Table<Client>()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

    /// <summary>
    /// Update a client
    /// </summary>
    public Task<int> UpdateAsync(Client client) =>
        _database.UpdateAsync(client);

    /// <summary>
    /// Delete a client
    /// </summary>
    public Task<int> DeleteAsync(Client client) =>
        _database.DeleteAsync(client);

    /// <summary>
    /// Archive client
    /// </summary>
    public Task ArchiveAsync(Client client)
    {
        client.IsArchived = true;
        return UpdateAsync(client);
    }
    
    /// <summary>
    /// Get archived clients
    /// </summary>
    public Task<List<Client>> GetArchivedAsync() =>
        _database.Table<Client>()
            .Where(c => c.IsArchived)
            .ToListAsync();
    
    /// <summary>
    /// Get all client without archived
    /// </summary>
    public Task<List<Client>> GetAllAsync() =>
        _database.Table<Client>()
            .Where(c => !c.IsArchived)
            .ToListAsync();

 
}