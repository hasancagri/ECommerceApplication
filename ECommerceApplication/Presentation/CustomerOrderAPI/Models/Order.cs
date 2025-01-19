namespace CustomerOrderAPI.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}
