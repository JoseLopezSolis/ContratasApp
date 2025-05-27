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

    // Obtiene todos los clientes no archivados
    public Task<List<Client>> GetAllAsync() =>
        _db.Table<Client>()
            .Where(c => !c.IsArchived)
            .ToListAsync();

    // Obtiene los clientes archivados
    public Task<List<Client>> GetArchivedAsync() =>
        _db.Table<Client>()
            .Where(c => c.IsArchived)
            .ToListAsync();

    // Obtiene un cliente por Id
    public Task<Client> GetByIdAsync(int id) =>
        _db.FindAsync<Client>(id);

    // Agrega un nuevo cliente
    public Task<int> AddAsync(Client client) =>
        _db.InsertAsync(client);

    // Actualiza un cliente existente
    public Task<int> UpdateAsync(Client client) =>
        _db.UpdateAsync(client);
    
    // Elimina un cliente
    public Task<int> DeleteAsync(Client client) =>
        _db.DeleteAsync(client);

    // Archiva un cliente (marca IsArchived = true)
    public async Task ArchiveAsync(Client client)
    {
        client.IsArchived = true;
        await _db.UpdateAsync(client);
    }
}