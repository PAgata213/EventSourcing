namespace ESProj.Domain.Common;
public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : ValueObject
{
	private readonly List<DomainEvent> _events = new();
	public int Version { get; protected set; }
	public IReadOnlyList<DomainEvent> GetEvents()
	=> _events;

	public void Apply(DomainEvent @event)
	{
		When(@event);
		_events.Add(@event);
	}

	public abstract void When(DomainEvent @event);
}