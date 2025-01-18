namespace Domain.Outbox;

public record AddToBasketDomainEvent(Guid CustomerId, Guid ProductId)
    : IDomainEvent;
