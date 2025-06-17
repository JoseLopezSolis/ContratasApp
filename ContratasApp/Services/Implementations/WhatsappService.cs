using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;

namespace ContratasApp.Services.Implementations;

public class WhatsappService: IWhatsappService
{
    private readonly HttpClient _httpClient;

    public WhatsappService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5268") // Usa la IP local o dominio real
        };
    }

    public async Task<bool> SendReminderAsync(WhatsappReminderMessage message)
        {
            var json = JsonSerializer.Serialize(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
        
            try
            {
                var response = await _httpClient.PostAsync("/send-template-named", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error enviando mensaje: {ex.Message}");
                return false;
            }
        } 
}