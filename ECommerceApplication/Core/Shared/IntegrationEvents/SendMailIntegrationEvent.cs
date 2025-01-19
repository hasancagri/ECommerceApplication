namespace Shared.IntegrationEvents;

public class SendMailIntegrationEvent
{
    public string Email { get; set; }
    public string ProductName { get; set; }
    public string CustomerName { get; set; }
}
