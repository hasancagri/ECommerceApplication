namespace Domain.Outbox;

public record ChangeQuantityDomainEvent(Guid CustomerId, Guid ProductId, int Quantity)
    : IDomainEvent;
