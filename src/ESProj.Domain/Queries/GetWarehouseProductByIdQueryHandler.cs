using ESProj.Domain.Aggregates;
using ESProj.Domain.Repository;

using MediatR;

namespace ESProj.Domain.Queries;
internal class GetWarehouseProductByIdQueryHandler(IWarehouseProductRepository warehouseProductRepository)
	: IRequestHandler<GetWarehouseProductByIdQuery, WarehouseProduct?>
{
	public async Task<WarehouseProduct?> Handle(GetWarehouseProductByIdQuery request, CancellationToken cancellationToken)
		=> await warehouseProductRepository.GetByIdAsync(request.WarehouseProductId);
}
