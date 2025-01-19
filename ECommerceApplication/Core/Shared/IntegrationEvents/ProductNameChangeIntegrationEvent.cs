namespace Shared.IntegrationEvents;

public class ProductNameChangeIntegrationEvent
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
}
