using ESProj.Domain.Common;
using ESProj.Domain.Events;
using ESProj.Domain.VO;

namespace ESProj.Domain.Aggregates;
public class WarehouseProduct : AggregateRoot<WarehouseProductId>
{
	public WarehouseId WarehouseId { get; protected set; }
	public ProductId ProductId { get; protected set; }
	public int Quantity { get; private set; } = 0;

	public static WarehouseProduct Create(WarehouseId warehouseId, ProductId productId)
	{
		var warehouseProduct = new WarehouseProduct()
		{
			Id = new WarehouseProductId(Guid.NewGuid()),
			ProductId = productId,
			WarehouseId = warehouseId,
			Quantity = 0
		};

		warehouseProduct.Apply(new WarehouseProductCreated()
		{
			Version = 1,
			WarehouseId = warehouseId,
			ProductId = productId,
			Quantity = 0
		});

		return warehouseProduct;
	}

	public void AddQuantity(int quantity)
		=> Apply(new WarehouseProductQuantityAdded
		{
			Version = Version + 1,
			WarehouseId = WarehouseId,
			ProductId = ProductId,
			Quantity = quantity
		});

	public void RemoveQuantity(int quantity)
	{
		if(Quantity < quantity)
		{
			throw new Exception("Not enough quantity");
		}

		Apply(new WarehouseProductQuantityRemoved
		{
			Version = Version + 1,
			WarehouseId = WarehouseId,
			ProductId = ProductId,
			Quantity = quantity
		});
	}

	public override void When(DomainEvent @event)
	{
		switch(@event)
		{
			case WarehouseProductCreated t:
				OnCreated(t);
				break;

			case WarehouseProductQuantityAdded t:
				OnProductQuantityAdded(t);
				break;

			case WarehouseProductQuantityRemoved t:
				OnProductQuantityRemoved(t);
				break;
		}
		Version = @event.Version;
	}

	private void OnCreated(WarehouseProductCreated warehouseProductCreated)
	{
		Id = new(warehouseProductCreated.Id);
		WarehouseId = warehouseProductCreated.WarehouseId;
		ProductId = warehouseProductCreated.ProductId;
		Quantity = warehouseProductCreated.Quantity;
	}

	private void OnProductQuantityAdded(WarehouseProductQuantityAdded warehouseProductQuantityAdded)
		=> Quantity += warehouseProductQuantityAdded.Quantity;

	private void OnProductQuantityRemoved(WarehouseProductQuantityRemoved warehouseProductQuantityAdded)
		=> Quantity -= warehouseProductQuantityAdded.Quantity;
}