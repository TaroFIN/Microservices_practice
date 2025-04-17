namespace Ordering.Domain.Abstractions;

public interface IAggregate<T> : IEntity<T>, IAggregate
{
    new T Id { get; set; }
}

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}

