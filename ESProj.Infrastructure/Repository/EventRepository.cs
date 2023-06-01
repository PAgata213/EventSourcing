using System.Text;

using AutoMapper;

using ESProj.Domain.Common;
using ESProj.Infrastructure.Common;

using EventStore.Client;
using Newtonsoft.Json;

namespace ESProj.Infrastructure.Repository;

internal class EventRepository(EventStoreClient _client, IMapper _mapper) : IEventRepository
{
	public async Task<T?> Get<T, TKey, TSnapshot>(string streamName)
		where T : AggregateRoot<TKey>, new()
		where TKey : ValueObject
		where TSnapshot : AggregateSnapshot
	{
		var aggregateSnapshot = await GetAggregateSnapshotOrDefault<TSnapshot>(streamName);
		var aggregate = _mapper.Map<T>(aggregateSnapshot) ?? new T();

		var readStreamResult = _client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.FromInt64(aggregate.Version));
		var eventsData = await readStreamResult.ToListAsync();

		if(aggregateSnapshot == null && (eventsData?.Count ?? 0) == 0)
		{
			return null;
		}

		foreach(var @eventData in eventsData!)
		{
			var eventType = Type.GetType(@eventData.Event.EventType);
			ArgumentNullException.ThrowIfNull(eventType);
			var @event = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(@eventData.Event.Data.Span), eventType);
			ArgumentNullException.ThrowIfNull(@event);
			aggregate.When((DomainEvent)@event);
		}

		return aggregate;
	}

	public async Task Save<T, TKey, TSnapshot>(string streamName, T aggregate)
		where T : AggregateRoot<TKey>, new()
		where TKey : ValueObject
		where TSnapshot : AggregateSnapshot
	{
		var events = aggregate.GetEvents();
		var eventData = events.Select(@event =>
		{
			var eventType = @event.GetType();
			var data = JsonConvert.SerializeObject(@event);
			return new EventData(Uuid.NewUuid(), eventType.AssemblyQualifiedName!, Encoding.UTF8.GetBytes(data));
		});

		await _client.AppendToStreamAsync(streamName, StreamState.Any, eventData);

		foreach(var @event in events.Reverse())
		{
			if(@event.Version % 5 == 0)
			{
				await CreateAggregateSnapshot<T, TKey, TSnapshot>(streamName, aggregate);
				break;
			}
		}
	}

	private async Task<TSnapshot?> GetAggregateSnapshotOrDefault<TSnapshot>(string streamName)
		where TSnapshot : AggregateSnapshot
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

		return JsonConvert.DeserializeObject<TSnapshot>(Encoding.UTF8.GetString(spanshotData![0].Event.Data.Span));
	}

	private async Task CreateAggregateSnapshot<T, TKey, TSnapshot>(string streamName, T aggregate)
		where T : AggregateRoot<TKey>, new()
		where TKey : ValueObject
		where TSnapshot : AggregateSnapshot
	{
		var aggregateType = aggregate.GetType();
		var data = JsonConvert.SerializeObject(_mapper.Map<TSnapshot>(aggregate));
		var eventData = new EventData(Uuid.NewUuid(), typeof(TSnapshot).AssemblyQualifiedName!, Encoding.UTF8.GetBytes(data));

		await _client.AppendToStreamAsync(GetSnapshotStreamName(streamName), StreamState.Any, new[] { eventData });
	}

	private static string GetSnapshotStreamName(string streamName)
		=> $"{streamName}-snapshot";
}
