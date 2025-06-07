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
                opt => opt.MapFrom(src => src.ProductIngredients!.Select(pi => pi.Ingredient)))
            .ForMember(dest => dest.Variants,
                opt => opt.MapFrom(src => src.Variants));

        CreateMap<ProductCreateModel, ProductEntity>()
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
            .ForMember(dest => dest.ProductIngredients, opt => opt.Ignore());

        CreateMap<ProductImageEntity, ProductImageItemModel>();
        CreateMap<ProductEntity, ProductVariantItemModel>()
            .ForMember(dest => dest.ProductImages, opt => opt
            .MapFrom(x => x.ProductImages!.OrderBy(p => p.Priority)));

        CreateMap<ProductEditModel, ProductEntity>()
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
            .ForMember(dest => dest.ProductIngredients, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.Ignore());

        CreateMap<ProductEntity, ProductEditModel>()
            .ForMember(dest => dest.ExistingImageIds,
                opt => opt.MapFrom(src => src.ProductImages!.OrderBy(p => p.Priority).Select(p => p.Id)))
            .ForMember(dest => dest.IngredientIds,
                opt => opt.MapFrom(src => src.ProductIngredients!.Select(pi => pi.IngredientId)))
            .ForMember(dest => dest.NewImages, opt => opt.Ignore()); 
    }
}
