namespace Domain.Products;

public record ProductCreatedDomainEvent(Guid Id, int Barcode, string Description, int Quantity, decimal Price)
    : IDomainEvent;
