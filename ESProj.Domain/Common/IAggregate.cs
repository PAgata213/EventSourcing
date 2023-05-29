namespace ESProj.Domain.Common;
public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : ValueObject
{
  public int Version { get; protected set; } = 0;

  private readonly List<DomainEvent> _events = new();

  public IReadOnlyList<DomainEvent> GetEvents()
    => _events;

  public void Apply(DomainEvent @event)
  {
    When(@event);
    _events.Add(@event);
  }

  public abstract void When(DomainEvent @event);
}
