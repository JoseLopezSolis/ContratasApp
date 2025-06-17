namespace ContratasApp.Models;

public class WhatsappReminderMessage
{
    public string To { get; set; }
    public string TemplateName { get; set; } = "recordatorio_pago";
    public string LanguageCode { get; set; } = "es_MX";
    public string ClientName { get; set; }
    public string Amount { get; set; }
    public string LimitDate { get; set; }
}