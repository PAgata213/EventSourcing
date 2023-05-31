using System.Text.Json;

using ESProj.Domain.Common;

using EventStore.Client;

namespace ESProj.Infrastructure.Repository;

internal class EventRepository(EventStoreClient _client) : IEventRepository
{
	public async Task<T?> Get<T, TKey>(string streamName)
		where T : AggregateRoot<TKey>, new()
		where TKey : ValueObject
	{
		var aggregate = await GetSnapshotOrDefault<T, TKey>(streamName);

		if(aggregate == null)
		{
			aggregate = new T();
		}

		var readStreamResult = _client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.FromInt64(aggregate.Version));
		var eventsData = await readStreamResult.ToListAsync();

		if((eventsData?.Count ?? 0) == 0)
		{
			return null;
		}

		foreach(var @eventData in eventsData!)
		{
			var eventType = Type.GetType(@eventData.Event.EventType);
			ArgumentNullException.ThrowIfNull(eventType);
			var @event = JsonSerializer.Deserialize(@eventData.Event.Data.Span, eventType);
			ArgumentNullException.ThrowIfNull(@event);
			aggregate.When((DomainEvent)@event);
		}

		return aggregate;
	}

	public async Task Save<T, TKey>(string streamName, T aggregate)
		where T : AggregateRoot<TKey>, new()
		where TKey : ValueObject
	{
		var events = aggregate.GetEvents();
		var eventData = events.Select(@event =>
		{
			var eventType = @event.GetType();
			var data = JsonSerializer.SerializeToUtf8Bytes(@event, eventType);
			return new EventData(Uuid.NewUuid(), eventType.AssemblyQualifiedName!, data);
		});

		await _client.AppendToStreamAsync(streamName, StreamState.Any, eventData);

		foreach(var @event in events.Reverse())
		{
			if(@event.Version % 5 == 0)
			{
				await CreateSnapshot<T, TKey>(streamName, aggregate);
				break;
			}
		}
	}

	private async Task<T?> GetSnapshotOrDefault<T, TKey>(string streamName)
		where T : AggregateRoot<TKey>, new()
		where TKey : ValueObject
	{
		var readStreamResult = _client.ReadStreamAsync(Direction.Backwards, GetSnapshotStreamName(streamName), StreamPosition.End, 1);
		if(await readStreamResult.ReadState == ReadState.StreamNotFound)
		{
			return null;
		}
		var spanshotData = await readStreamResult.ToListAsync();
		if(spanshotData?.Count == 0)
		{
			return null;
		}

		return JsonSerializer.Deserialize<T>(spanshotData![0].Event.Data.Span);
	}

	private async Task CreateSnapshot<T, TKey>(string streamName, T aggregate)
		where T : AggregateRoot<TKey>, new()
		where TKey : ValueObject
	{
		var aggregateType = aggregate.GetType();
		var data = JsonSerializer.SerializeToUtf8Bytes(aggregate, aggregateType);
		var eventData = new EventData(Uuid.NewUuid(), aggregateType.AssemblyQualifiedName!, data);

		await _client.AppendToStreamAsync(GetSnapshotStreamName(streamName), StreamState.Any, new[] { eventData });
	}

	private string GetSnapshotStreamName(string streamName)
		=> $"{streamName}-snapshot";
}
