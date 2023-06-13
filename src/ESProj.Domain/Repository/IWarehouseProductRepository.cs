using ESProj.Domain.Aggregates;
using ESProj.Domain.VO;

namespace ESProj.Domain.Repository;

public interface IWarehouseProductRepository
{
    public Task<WarehouseProduct?> GetByIdAsync(WarehouseProductId id);
    public Task SaveAsync(WarehouseProduct warehouseProduct);
}
