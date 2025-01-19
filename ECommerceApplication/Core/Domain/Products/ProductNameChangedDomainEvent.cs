namespace Domain.Products;

public record ProductNameChangedDomainEvent(Guid Id, string Name)
    : IDomainEvent;
