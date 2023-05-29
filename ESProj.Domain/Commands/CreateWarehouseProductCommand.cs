using ESProj.Domain.VO;

using MediatR;

namespace ESProj.Domain.Commands;
public record CreateWarehouseProductCommand : IRequest<WarehouseProductId>
{
	public required WarehouseId WarehouseId { get; init; }
	public required ProductId ProductId { get; init; }
}
