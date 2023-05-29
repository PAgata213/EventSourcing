using ESProj.Domain.Aggregates;
using ESProj.Domain.VO;

using MediatR;

namespace ESProj.Domain.Queries;
public record GetWarehouseProductByIdQuery : IRequest<WarehouseProduct?>
{
	public required WarehouseProductId WarehouseProductId { get; init; }
}
