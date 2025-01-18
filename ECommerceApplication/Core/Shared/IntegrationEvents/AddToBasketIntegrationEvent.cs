namespace Shared.IntegrationEvents;

public class AddToBasketIntegrationEvent
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}
