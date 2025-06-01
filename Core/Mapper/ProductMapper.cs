using AutoMapper;
using Core.Models.Category;
using Core.Models.Ingredient;
using Core.Models.Product;
using Core.Models.ProductImage;
using Core.Models.ProductSize;
using Domain.Entities;

namespace Core.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductEntity, ProductItemModel>();
        CreateMap<CategoryEntity, CategoryItemModel>();
        CreateMap<ProductSizeEntity, ProductSizeItemModel>();
        CreateMap<ProductImageEntity, ProductImageItemModel>();
        CreateMap<IngredientEntity, IngredientItemModel>();
    }
}
