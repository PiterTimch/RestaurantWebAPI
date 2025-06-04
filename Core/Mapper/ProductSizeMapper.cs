using AutoMapper;
using Domain.Entities;
using Core.Models.Seeder;
using Core.Models.ProductSize;

namespace Core.Mapper;

public class ProductSizeMapper : Profile
{
    public ProductSizeMapper() { 
        CreateMap<SeederProductSizeModel, ProductSizeEntity>();
        CreateMap<ProductSizeEntity, ProductSizeItemModel>();

    }
}
