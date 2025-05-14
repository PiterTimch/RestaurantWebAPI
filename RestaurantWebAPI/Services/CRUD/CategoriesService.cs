using AutoMapper;
using MailKit;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Services.CRUD;

public class CategoriesService(AppDbRestaurantContext context, 
    IMapper mapper, IImageService imageService) : ICategoriesService
{
    public async Task<CategoryEntity> CreateAsync(CategoryCreateModel model)
    {
        var entity = await context.Categories.SingleOrDefaultAsync(x => x.Name == model.Name);
        if (entity != null)
        {
            return null;
        }

        entity = mapper.Map<CategoryEntity>(model);
        if (model.ImageFile != null)
            entity.Image = await imageService.SaveImageAsync(model.ImageFile);

        await context.Categories.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<IEnumerable<CategoryItemModel>> GetAllAsync()
    {
        var model = await mapper.ProjectTo<CategoryItemModel>(context.Categories).ToListAsync();
        return model ?? new List<CategoryItemModel>();
    }

    public async Task<CategoryEntity> GetByIdAsync(int id)
    {
        var model = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        return model;
    }
}
