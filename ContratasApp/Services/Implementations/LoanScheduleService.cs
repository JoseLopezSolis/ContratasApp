using ContratasApp.Enums;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;

namespace ContratasApp.Services.Implementations;

public class LoanScheduleService : ILoanScheduleService 
{
    public List<Payment> GenerateSchedule(Loan loan)
    {
        var payments = new List<Payment>();
        decimal loanAmount = loan.Amount;
        decimal installment = loanAmount * 0.10m;
        DateTime dueDate = loan.StartDate;

        switch (loan.Type)
        {
            case LoanType.Weekly:
                for (int i = 0; i < 13; i++)
                {
                    payments.Add(new Payment
                    {
                        LoanId = loan.Id,
                        DueDate = dueDate, // ⬅️ AQUÍ se genera la fecha
                        Amount = installment,
                        IsPaid = false
                    });
                    dueDate = dueDate.AddDays(7); // ⬅️ Fecha de la próxima semana
                }
                break;

            case LoanType.Monthly:
                decimal remaining = loanAmount;
                while (remaining > 0)
                {
                    decimal cuota = Math.Min(installment, remaining);
                    payments.Add(new Payment
                    {
                        LoanId = loan.Id,
                        DueDate = dueDate, // ⬅️ Fecha de este pago
                        Amount = cuota,
                        IsPaid = false
                    });

                    remaining -= cuota;
                    dueDate = dueDate.AddDays(30); // ⬅️ Próxima mensualidad
                }
                break;
        }

        return payments;
    }
}