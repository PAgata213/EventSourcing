using ESProj.Domain.Common;
using ESProj.Infrastructure.Common;

namespace ESProj.Infrastructure.Repository;
internal interface IEventRepository
{
  Task<T?> Get<T, TKey, TSnapshot>(string streamName) 
    where T : AggregateRoot<TKey>, new() 
    where TKey : ValueObject
		where TSnapshot : AggregateSnapshot;
  Task Save<T, TKey, TSnapshot>(string streamName, T aggregate)
    where T : AggregateRoot<TKey>, new()
    where TKey : ValueObject
		where TSnapshot : AggregateSnapshot;
}
