using BuildingBlocks.Domain;

namespace Shared.Domain;

public abstract class AggregateRoot
{
    private List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void Clear() => _domainEvents.Clear();
}
