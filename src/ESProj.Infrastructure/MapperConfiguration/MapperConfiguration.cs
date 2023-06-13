using AutoMapper;

using ESProj.Domain.Aggregates;
using ESProj.Domain.VO;
using ESProj.Infrastructure.Snapshots;

namespace ESProj.Infrastructure.MapperConfiguration;
public class MapperConfiguration : Profile
{
	public MapperConfiguration()
	{
		CreateMap<WarehouseProduct, WarehouseProductSnapshot>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
			.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.Id))
			.ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId.Id));

		CreateMap<WarehouseProductSnapshot, WarehouseProduct>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => new WarehouseProductId(src.Id)))
			.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => new ProductId(src.ProductId)))
			.ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => new WarehouseId(src.WarehouseId)));
	}
}
