using AutoMapper;
using Core.Models.Order;
using Domain.Entities;

namespace Core.Mapper;

public class OederMapper : Profile
{
    public OederMapper()
    {
        CreateMap<OrderItemEntity, OrderItemModel>()
            .ForMember(x => x.ProductImage, opt => opt
            .MapFrom(x => x.Product!.ProductImages!.OrderBy(x => x.Priority).First().Name))
            .ForMember(x => x.ProductName, opt => opt
            .MapFrom(x => x.Product!.Name));

        CreateMap<OrderEntity, OrderModel>()
            .ForMember(x => x.Status, opt => opt
            .MapFrom(x => x.OrderStatus!.Name));
    }
}
