namespace Domain.Outbox;

public record ClearBasketDomainEvent(Guid CustomerId)
    : IDomainEvent;
