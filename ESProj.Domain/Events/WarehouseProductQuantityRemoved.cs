﻿using ESProj.Domain.Common;
using ESProj.Domain.VO;

namespace ESProj.Domain.Events;

public record WarehouseProductQuantityRemoved : DomainEvent
{
	public required WarehouseId WarehouseId { get; init; }
	public required ProductId ProductId { get; init; }
	public required int Quantity { get; init; }
}
