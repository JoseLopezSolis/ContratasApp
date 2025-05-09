using System.Threading.Tasks.Dataflow;
using ContratasApp.Enums;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using SQLite;

namespace ContratasApp.Services.Implementations;

public class ContractService : IContractService
{
    
    readonly SQLiteAsyncConnection _db;

    public ContractService(SQLiteAsyncConnection db)
    {
        _db = db;
        //Creating the tables when class is called
        _db.CreateTableAsync<LoanContract>().Wait();
        _db.CreateTableAsync<PaymentSchedule>().Wait();
    }
    
    public async Task<int> CreateAsync(LoanContract contract)
    {
        // 1) Inserta el contrato y captura su Id
        var id = await _db.InsertAsync(contract);

        // 2) Genera las cuotas según el tipo
        var schedules = new List<PaymentSchedule>();

        if (contract.Type == LoanType.Weekly)
        {
            var cuota = contract.Principal * 0.10m;
            for (int i = 1; i <= 13; i++)
                schedules.Add(new PaymentSchedule {
                    ContractId = id,
                    DueDate    = contract.StartDate.AddDays(7 * i),
                    Amount     = cuota,
                    IsPaid     = false
                });
        }
        else // MonthlyInterest
        {
            var interes = contract.Principal * 0.10m;
            // Dejanos generar 12 meses de interés
            for (int m = 1; m <= 12; m++)
                schedules.Add(new PaymentSchedule {
                    ContractId = id,
                    DueDate    = contract.StartDate.AddMonths(m),
                    Amount     = interes,
                    IsPaid     = false
                });
        }

        // 3) Inserta todas las cuotas en bloque
        await _db.InsertAllAsync(schedules);
        return id;
    }
    
    public Task<List<LoanContract>> GetByClientIdAsync(int clientId) =>
        _db.Table<LoanContract>()
            .Where(c => c.ClientId == clientId)
            .ToListAsync();

    public Task<List<PaymentSchedule>> GetPaymentSchedulesAsync(int contractId) =>
        _db.Table<PaymentSchedule>()
            .Where(p => p.ContractId == contractId)
            .ToListAsync();


    public async Task MarkPaymentAsPaidAsync(PaymentSchedule payment)
    {
        // Marca como pagado
        payment.IsPaid = true;
        await _db.UpdateAsync(payment);

        // Si ya no quedan cuotas pendientes, cierra el contrato
        var pendientes = await _db.Table<PaymentSchedule>()
            .Where(p => p.ContractId == payment.ContractId && !p.IsPaid)
            .CountAsync();

        if (pendientes == 0)
            await CloseContractAsync(payment.ContractId);
    }

    public async Task CloseContractAsync(int contractId)
    {
        var contrato = await _db.FindAsync<LoanContract>(contractId);
        if (contrato == null) return;
        contrato.IsClosed = true;
        await _db.UpdateAsync(contrato);
    }
}