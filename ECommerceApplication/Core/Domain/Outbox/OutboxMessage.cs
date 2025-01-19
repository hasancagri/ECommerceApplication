namespace Domain.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
}
