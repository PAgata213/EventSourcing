using ESProj.Domain.Common;
using ESProj.Domain.VO;

namespace ESProj.Domain.Events;
internal record WarehouseProductCreated : DomainEvent
{
	public required WarehouseId WarehouseId { get; init; }
	public required ProductId ProductId { get; init; }
	public int Quantity { get; init; } = 0;
}
