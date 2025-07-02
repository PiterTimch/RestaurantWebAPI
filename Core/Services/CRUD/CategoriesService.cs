using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using Core.Interfaces;
using Core.Models.Category;
using Core.Models.Common;
using Core.Models.Search.Params;

namespace Core.Services.CRUD;

public class CategoriesService(
    AppDbRestaurantContext context,
    IMapper mapper,
    IImageService imageService) : ICategoriesService
{
    public async Task<CategoryItemModel> CreateAsync(CategoryCreateModel model)
    {
        var entity = mapper.Map<CategoryEntity>(model);
        if (model.ImageFile != null)
        {
            entity.Image = await imageService.SaveImageAsync(model.ImageFile);
        }
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
            .Where(x => !x.IsDeleted)
            .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<PaginationModel<CategoryItemModel>> GetAllAsync(CategorySearchModel searchModel)
    {
        var query = context.Categories.AsQueryable();

        if (searchModel.Id.HasValue)
        {
            query = query.Where(x => x.Id == searchModel.Id.Value);
        }
        else
        {
            if (!String.IsNullOrEmpty(searchModel.Name))
            {
                string searchName = searchModel.Name.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchName));
            }
            if (!String.IsNullOrEmpty(searchModel.Slug))
            {
                string searchSlug = searchModel.Slug.Trim().ToLower();
                query = query.Where(x => x.Slug.ToLower().Contains(searchSlug));
            }
        }

        int skip = (searchModel.PageNumber - 1) * searchModel.ItemsPerPage;

        var items = await query
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(searchModel.ItemsPerPage)
            .ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        var model = new PaginationModel<CategoryItemModel>
        {
            Items = items,
            ItemPerPage = searchModel.ItemsPerPage,
            CurrentPge = searchModel.PageNumber,
            TotalPge = (int)Math.Ceiling((double)await query.CountAsync() / searchModel.ItemsPerPage)
        };

        return model;
    }

    public async Task DeleteAsync(CategoryDeleteModel model)
    {
        var entity = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);

        entity.IsDeleted = true;

        await context.SaveChangesAsync();

    }
}
