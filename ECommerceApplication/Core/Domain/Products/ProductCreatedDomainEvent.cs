namespace Domain.Products;

public record ProductCreatedDomainEvent(Guid id, int barcode, string description, int quantity, decimal price)
    : IDomainEvent;
