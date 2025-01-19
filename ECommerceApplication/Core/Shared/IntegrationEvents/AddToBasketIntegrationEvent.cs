namespace Shared.IntegrationEvents;

public class AddToBasketIntegrationEvent
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}
