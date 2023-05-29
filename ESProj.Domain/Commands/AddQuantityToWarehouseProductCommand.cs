using ErrorOr;

using ESProj.Domain.VO;

using MediatR;

namespace ESProj.Domain.Commands;
public record AddQuantityToWarehouseProductCommand : IRequest<ErrorOr<bool>>
{
	public required WarehouseProductId WarehouseProductId { get; init; }
	public required int Quantity { get; init; }
}