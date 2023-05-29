﻿using System.Text.Json;
using ESProj.Domain.Common;
using EventStore.Client;

namespace ESProj.Infrastructure.Repository;

internal class EventRepository(EventStoreClient _client) : IEventRepository
{
  public async Task<T?> Get<T, TKey>(string streamName) 
    where T : AggregateRoot<TKey>, new()
    where TKey : ValueObject
  {
    var readStreamResult = _client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);
    var eventsData = await readStreamResult.ToListAsync();

    if((eventsData?.Count ?? 0) == 0)
    {
      return null;
    }

    var aggregate = new T();

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
  }
}
