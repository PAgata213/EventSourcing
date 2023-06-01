using ESProj.Infrastructure.Common;

namespace ESProj.Infrastructure.Snapshots;
internal class WarehouseProductSnapshot : AggregateSnapshot
{
	public Guid WarehouseId { get; set; }
	public Guid ProductId { get; set; }
	public int Quantity { get; set; }
}
