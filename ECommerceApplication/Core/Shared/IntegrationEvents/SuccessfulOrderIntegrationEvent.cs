namespace Shared.IntegrationEvents;

public class SuccessfulOrderIntegrationEvent
{
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
}
