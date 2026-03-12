namespace Ordering.Domain.Abstractions;
public class Agregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomaninEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomaninEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomaninEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IDomaninEvent[] ClearDomaninEvents()
    {
        IDomaninEvent[] dequeuedEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeuedEvents;
    }
}
