using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ErrorOr;

using ESProj.Domain.Repository;

using MediatR;

namespace ESProj.Domain.Commands;
internal class RemoveQuantityToWarehouseProductCommandHandler(IWarehouseProductRepository warehouseProductRepository)
	: IRequestHandler<RemoveQuantityToWarehouseProductCommand, ErrorOr<bool>>
{
	public async Task<ErrorOr<bool>> Handle(RemoveQuantityToWarehouseProductCommand request, CancellationToken cancellationToken) 
	{
		var warehouseProduct = await warehouseProductRepository.GetByIdAsync(request.WarehouseProductId);

		if(warehouseProduct is null)
		{
			return Error.NotFound();
		}

		warehouseProduct.RemoveQuantity(request.Quantity);

		await warehouseProductRepository.SaveAsync(warehouseProduct);

		return true;
	}
}
