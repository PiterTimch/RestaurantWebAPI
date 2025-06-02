using AutoMapper;
using Core.Models.Category;
using Core.Models.Ingredient;
using Core.Models.Product;
using Core.Models.ProductImage;
using Core.Models.ProductSize;
using Domain.Entities;
using System.Linq;

namespace Core.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductEntity, ProductItemModel>()
        .ForMember(dest => dest.ProductIngredients, opt =>
            opt.MapFrom(src =>
                src.ProductIngredients != null
                    ? src.ProductIngredients
                        .Where(pi => pi.ProductId == src.Id)
                        .Select(pi => pi.Ingredient)
                    : new List<IngredientEntity>()
            ));

        CreateMap<CategoryEntity, CategoryItemModel>();
        CreateMap<ProductSizeEntity, ProductSizeItemModel>();
        CreateMap<ProductImageEntity, ProductImageItemModel>();
        CreateMap<IngredientEntity, IngredientItemModel>();
    }
}
