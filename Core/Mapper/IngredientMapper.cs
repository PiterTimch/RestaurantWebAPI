using AutoMapper;
using Domain.Entities;
using Core.Models.Category;
using Core.Models.Seeder;
using Core.Models.Ingredient;

namespace Core.Mapper;

public class IngredientMapper : Profile
{
    public IngredientMapper() { 
        CreateMap<SeederIngredientModel, IngredientEntity>();

        CreateMap<IngredientEntity, IngredientItemModel>();

        CreateMap<IngredientCreateModel, IngredientEntity>()
            .ForMember(x => x.Image, opt => opt.Ignore());
    }
}
