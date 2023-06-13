using ErrorOr;

using ESProj.Domain.Repository;

using MediatR;

namespace ESProj.Domain.Commands;
internal class AddQuantityToWarehouseProductCommandHandler(IWarehouseProductRepository warehouseProductRepository)
	: IRequestHandler<AddQuantityToWarehouseProductCommand, ErrorOr<bool>>
{
	public async Task<ErrorOr<bool>> Handle(AddQuantityToWarehouseProductCommand request, CancellationToken cancellationToken) 
	{
		var warehouseProduct = await warehouseProductRepository.GetByIdAsync(request.WarehouseProductId);

		if(warehouseProduct == null)
		{
			return Error.NotFound();
		}

		warehouseProduct.AddQuantity(request.Quantity);

		await warehouseProductRepository.SaveAsync(warehouseProduct);
		return true;
	}
}
