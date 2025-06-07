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
    public Task<List<Client>> GetAllActiveClients() =>
        _db.Table<Client>()
            .Where(client => !client.IsArchived)
            .ToListAsync();

    public Task<List<Client>> GetArchivedAsync() =>
        _db.Table<Client>()
            .Where(client => client.IsArchived)
            .ToListAsync();

    public Task<Client> GetByIdAsync(int id) =>
        _db.FindAsync<Client>(id);

    public Task<int> AddAsync(Client client) =>
        _db.InsertAsync(client);

    public Task<int> UpdateAsync(Client client) =>
        _db.UpdateAsync(client);
    
    public Task<int> DeleteAsync(Client client) =>
        _db.DeleteAsync(client);

    public async Task ArchiveAsync(Client client)
    {
        client.IsArchived = true;
        await _db.UpdateAsync(client);
    }
}