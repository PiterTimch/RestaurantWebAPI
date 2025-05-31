using AutoMapper;
using Domain.Entities;
using Core.Models.Seeder;

namespace Core.Mapper;

public class ProductSizeMapper : Profile
{
    public ProductSizeMapper() { 
        CreateMap<SeederProductSizeModel, ProductSizeEntity>();
        
    }
}
