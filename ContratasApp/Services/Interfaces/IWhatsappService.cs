using ContratasApp.Models;

namespace ContratasApp.Services.Interfaces;

public interface IWhatsappService
{
    public Task<bool> SendReminderAsync(WhatsappReminderMessage message);
}