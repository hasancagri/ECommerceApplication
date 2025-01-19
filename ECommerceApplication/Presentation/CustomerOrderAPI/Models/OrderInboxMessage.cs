namespace CustomerOrderAPI.Models;

public class OrderInboxMessage
{
    public Guid Id { get; set; }
    public Guid IdempotentToken { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public bool Processed { get; set; }
}
