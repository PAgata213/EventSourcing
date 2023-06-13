using ErrorOr;

using ESProj.Domain.Aggregates;
using ESProj.Domain.Commands;
using ESProj.Domain.Queries;
using ESProj.Domain.Repository;
using ESProj.Domain.VO;
using ESProj.Infrastructure.Repository;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ESProj.Application.API;

public static class WarehouseProductAPI
{
	public static void MapWarehouseProductEndpoints(this WebApplication app)
	{
		app.MapGet("/api/warehouseproduct/{warehouseProductId}", GetWarehouseProductById);
		app.MapPost("/api/warehouseproduct/Create/{warehouseId}/{productId}", CreateWarehouseProduct);
		app.MapPost("/api/warehouseproduct/{warehouseProductId}/addquantity/{quantity}", AddQuantityToWarehouseProduct);
		app.MapPost("/api/warehouseproduct/{warehouseProductId}/removequantity/{quantity}", RemoveQuantityToWarehouseProduct);
	}

	private static async Task<IResult> GetWarehouseProductById([FromServices]ISender mediator, Guid warehouseProductId)
	{
		var warehouseProduct = await mediator.Send(new GetWarehouseProductByIdQuery
		{
			WarehouseProductId = new(warehouseProductId)
		});

		if(warehouseProduct is null)
		{
			return Results.NotFound();
		}
		return Results.Ok(warehouseProduct);
	}

	private static async Task<IResult> CreateWarehouseProduct([FromServices]ISender mediator, Guid warehouseId, Guid productId)
	{
		var warehouseProductId = await mediator.Send(new CreateWarehouseProductCommand 
		{
			WarehouseId = new(warehouseId),
			ProductId = new(productId),
		});

		if(warehouseProductId is null)
		{
			return Results.BadRequest();
		}
		
		return TypedResults.Created($"/api/warehouseproduct/{warehouseProductId.Value}");
	}

	private static async Task<IResult> AddQuantityToWarehouseProduct([FromServices]ISender mediator, Guid warehouseProductId, int quantity)
	{
		var result = await mediator.Send(new AddQuantityToWarehouseProductCommand
		{
			Quantity = quantity,
			WarehouseProductId = new WarehouseProductId(warehouseProductId)
		});

		if(result.IsError)
		{
			return Results.BadRequest(result.Errors);
		}

		return Results.Ok();
	}

	private static async Task<IResult> RemoveQuantityToWarehouseProduct([FromServices]ISender mediator, Guid warehouseProductId, int quantity)
	{
		var result = await mediator.Send(new RemoveQuantityToWarehouseProductCommand
		{
			Quantity = quantity,
			WarehouseProductId = new WarehouseProductId(warehouseProductId)
		});

		if(result.IsError)
		{
			return Results.BadRequest(result.Errors);
		}

		return Results.Ok();
	}
}
