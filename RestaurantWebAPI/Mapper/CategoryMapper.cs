using AutoMapper;
using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Models.Category;
using RestaurantWebAPI.Models.Seeder;

namespace RestaurantWebAPI.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper() { 
        CreateMap<SeederCategoryModel, CategoryEntity>();
        CreateMap<CategoryItemModel, CategoryEntity>()
            .ReverseMap();
    }
}
