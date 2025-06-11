using ContratasApp.Enums;
using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface ILoanScheduleService
{
    public List<Payment> GenerateSchedule(Loan loan);
}