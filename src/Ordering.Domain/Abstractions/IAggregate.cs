namespace Ordering.Domain.Abstractions;

public interface IAggregate<T> : IAggregate, IEntity<T>
{

}

public interface IAggregate : IEntity
{
    IReadOnlyCollection<IDomaninEvent> DomainEvents { get; }

    IDomaninEvent[] ClearDomaninEvents();
 }

