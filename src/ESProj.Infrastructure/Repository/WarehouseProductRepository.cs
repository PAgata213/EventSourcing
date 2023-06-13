using ESProj.Domain.Aggregates;
using ESProj.Domain.Repository;
using ESProj.Domain.VO;
using ESProj.Infrastructure.Snapshots;

namespace ESProj.Infrastructure.Repository;
internal class WarehouseProductRepository(IEventRepository eventRepository) : RepositoryBase, IWarehouseProductRepository
{
	public async Task<WarehouseProduct?> GetByIdAsync(WarehouseProductId id)
		=> await eventRepository.Get<WarehouseProduct, WarehouseProductId, WarehouseProductSnapshot>
		(GenerateStreamName<WarehouseProduct, WarehouseProductId>(id.Value));

	public async Task SaveAsync(WarehouseProduct WarehouseProduct)
		=> await eventRepository.Save<WarehouseProduct, WarehouseProductId, WarehouseProductSnapshot>
		(GenerateStreamName<WarehouseProduct, WarehouseProductId>(WarehouseProduct.Id.Value), WarehouseProduct);
}
