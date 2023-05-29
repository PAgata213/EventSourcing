using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESProj.Domain.Common;

namespace ESProj.Infrastructure.Repository;
internal class RepositoryBase
{
	protected static string GenerateStreamName<T, TKey>(Guid id) 
		where T : AggregateRoot<TKey> 
		where TKey : ValueObject
	{
		return $"{typeof(T).Name}-{id}";
	}
}
