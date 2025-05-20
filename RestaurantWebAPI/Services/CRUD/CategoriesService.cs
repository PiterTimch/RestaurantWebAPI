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

        model.Slug = model.Slug.Trim().ToLower().Replace(" ", "-");

        entity = mapper.Map<CategoryEntity>(model);
        if (model.ImageFile != null)
            entity.Image = await imageService.SaveImageAsync(model.ImageFile);

        await context.Categories.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<CategoryEntity> UpdateAsync(CategoryEditModel model) 
    {
        var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (existing == null)
            return null;

        model.Slug = model.Slug.Trim().ToLower().Replace(" ", "-");

        var duplicate = await context.Categories
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
        duplicate = await context.Categories
                .FirstOrDefaultAsync(x => x.Slug == model.Slug && x.Id != model.Id);
        if (duplicate != null)
            return null;

        existing = mapper.Map(model, existing);

        if (model.ImageFile != null)
        {
            await imageService.DeleteImageAsync(existing.Image);
            existing.Image = await imageService.SaveImageAsync(model.ImageFile);
        }
        await context.SaveChangesAsync();

        return existing;
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

    public async Task<CategoryEntity> GetBySlugAsync(string slug)
    {
        return await context.Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public Task DeleteAsync(int id)
    {
        var entity = context.Categories.FirstOrDefault(x => x.Id == id);
        if (entity != null)
        {
            if (entity.Image != null)
            {
                imageService.DeleteImageAsync(entity.Image);
            }
            context.Categories.Remove(entity);
            return context.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }
}
