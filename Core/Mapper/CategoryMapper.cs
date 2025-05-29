using AutoMapper;
using Domain.Entities;
using Core.Models.Category;
using Core.Models.Seeder;

namespace Core.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper() { 
        CreateMap<SeederCategoryModel, CategoryEntity>();
        CreateMap<CategoryItemModel, CategoryEntity>()
            .ReverseMap();

        CreateMap<CategoryCreateModel, CategoryEntity>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name.Trim()))
            .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.Slug.Trim()))
            .ForMember(x => x.Image, opt => opt.Ignore());

        CreateMap<CategoryEditModel, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryEditModel>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name.Trim()))
            .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.Slug.Trim()))
            .ForMember(x => x.ImageFile, opt => opt.Ignore());
    }
}
