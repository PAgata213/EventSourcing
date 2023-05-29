using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESProj.Domain.Aggregates;
using ESProj.Domain.Repository;
using ESProj.Domain.VO;

namespace ESProj.Infrastructure.Repository;
internal class WarehouseProductRepository(IEventRepository eventRepository) : RepositoryBase, IWarehouseProductRepository
{
	public async Task<WarehouseProduct?> GetByIdAsync(WarehouseProductId id)
		=> await eventRepository.Get<WarehouseProduct, WarehouseProductId>(GenerateStreamName<WarehouseProduct, WarehouseProductId>(id.Value));

	public async Task SaveAsync(WarehouseProduct WarehouseProduct)
		=> await eventRepository.Save<WarehouseProduct, WarehouseProductId>(GenerateStreamName<WarehouseProduct, WarehouseProductId>(WarehouseProduct.Id.Value), WarehouseProduct);
}
