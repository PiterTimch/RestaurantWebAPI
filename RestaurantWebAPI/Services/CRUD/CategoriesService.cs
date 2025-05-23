using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Services.CRUD;

public class CategoriesService(
    AppDbRestaurantContext context,
    IMapper mapper,
    IImageService imageService) : ICategoriesService
{
    public async Task<CategoryItemModel> CreateAsync(CategoryCreateModel model)
    {
        var entity = mapper.Map<CategoryEntity>(model);
        await context.Categories.AddAsync(entity);
        await context.SaveChangesAsync();

        var mapped = mapper.Map<CategoryItemModel>(entity);
        return mapped;
    }

    public async Task<CategoryItemModel> UpdateAsync(CategoryEditModel model)
    {

        var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
        existing = mapper.Map(model, existing);

        if (model.ImageFile != null)
        {
            await imageService.DeleteImageAsync(existing.Image);
            existing.Image = await imageService.SaveImageAsync(model.ImageFile);
        }

        await context.SaveChangesAsync();

        var mapped = mapper.Map<CategoryItemModel>(existing);
        return mapped;
    }

    public async Task<CategoryItemModel> GetByIdAsync(int id)
    {
        var model = await context.Categories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return model;
    }

    public async Task<CategoryItemModel> GetBySlugAsync(string slug)
    {
        var model = await context.Categories
            .AsNoTracking()
            .Where(x => x.Slug == slug)
            .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return model;
    }

    public async Task<IEnumerable<CategoryItemModel>> GetAllAsync()
    {
        return await context.Categories
            .AsNoTracking()
            .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task DeleteAsync(CategoryDeleteModel model)
    {
        var entity = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (!string.IsNullOrEmpty(entity.Image))
        {
            await imageService.DeleteImageAsync(entity.Image);
        }

        context.Categories.Remove(entity);
        await context.SaveChangesAsync();

    }
}
