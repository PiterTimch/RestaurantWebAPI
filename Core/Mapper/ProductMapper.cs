using AutoMapper;
using Core.Models.Category;
using Core.Models.Ingredient;
using Core.Models.Product;
using Core.Models.ProductImage;
using Core.Models.ProductSize;
using Domain.Entities;
using SixLabors.ImageSharp.ColorSpaces.Companding;
using System.Linq;

namespace Core.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductEntity, ProductItemModel>()
            .ForMember(dest => dest.ProductImages, opt => opt
            .MapFrom(x => x.ProductImages!.OrderBy(p => p.Priority)))
            .ForMember(dest => dest.ProductIngredients,
                opt => opt.MapFrom(src => src.ProductIngredients!.Select(pi => pi.Ingredient)));


        CreateMap<CategoryEntity, CategoryItemModel>();
        CreateMap<ProductSizeEntity, ProductSizeItemModel>();
        CreateMap<ProductImageEntity, ProductImageItemModel>();
        CreateMap<IngredientEntity, IngredientItemModel>();
    }
}
