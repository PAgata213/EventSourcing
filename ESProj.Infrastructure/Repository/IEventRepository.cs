using ESProj.Domain.Common;

namespace ESProj.Infrastructure.Repository;
internal interface IEventRepository
{
  Task<T?> Get<T, TKey>(string streamName) where T : AggregateRoot<TKey>, new() where TKey : ValueObject;
  Task Save<T, TKey>(string streamName, T aggregate) where T : AggregateRoot<TKey>, new() where TKey : ValueObject;
}
