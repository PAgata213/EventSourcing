using ESProj.Domain.Aggregates;
using ESProj.Domain.Repository;
using ESProj.Domain.VO;

using MediatR;

namespace ESProj.Domain.Commands;
internal class CreateWarehouseProductCommandHandler(IWarehouseProductRepository warehouseProductRepository)
	: IRequestHandler<CreateWarehouseProductCommand, WarehouseProductId>
{
	public async Task<WarehouseProductId> Handle(CreateWarehouseProductCommand request, CancellationToken cancellationToken) 
	{
		var warehouseProduct = WarehouseProduct.Create(request.WarehouseId, request.ProductId);
		await warehouseProductRepository.SaveAsync(warehouseProduct);
		return warehouseProduct.Id;
	}
}
