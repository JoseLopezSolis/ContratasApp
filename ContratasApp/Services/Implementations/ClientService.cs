using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using SQLite;

namespace ContratasApp.Services.Implementations;

public class ClientService: IClientService
{
    private readonly SQLiteAsyncConnection _db;

    public ClientService(SQLiteAsyncConnection db)
    {
        _db = db;
        _db.CreateTableAsync<Client>().Wait();
    }

    // GET list of active Clients
    public Task<List<Client>> GetAllAsync() =>
        _db.Table<Client>()
            .Where(c => !c.IsArchived)
            .ToListAsync();

    //GET list of arhived Clients
    public Task<List<Client>> GetArchivedAsync() =>
        _db.Table<Client>()
            .Where(c => c.IsArchived)
            .ToListAsync();

    // GET Client by id
    public Task<Client> GetByIdAsync(int id) =>
        _db.FindAsync<Client>(id);

    //ADD new Client
    public Task<int> AddAsync(Client client) =>
        _db.InsertAsync(client);

    //UPDATE Client list
    public Task<int> UpdateAsync(Client client) =>
        _db.UpdateAsync(client);
    
    //REMOVE Client
    public Task<int> DeleteAsync(Client client) =>
        _db.DeleteAsync(client);

    //ARCHIVE Client
    public async Task ArchiveAsync(Client client)
    {
        client.IsArchived = true;
        await _db.UpdateAsync(client);
    }
}