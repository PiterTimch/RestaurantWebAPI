using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Category;
using RestaurantWebAPI.Validators.Helpers;

namespace RestaurantWebAPI.Services.CRUD;

public class CategoriesService(
    AppDbRestaurantContext context,
    IMapper mapper,
    IImageService imageService) : ICategoriesService
{
    public async Task<Result<CategoryEntity>> CreateAsync(CategoryCreateModel model)
    {
        var result = new Result<CategoryEntity>();
        var errors = new List<Error>();

        var existingByName = await context.Categories
            .SingleOrDefaultAsync(x => x.Name == model.Name);
        if (existingByName != null)
            errors.Add(ErrorHelper.FieldError("Категорія з таким іменем вже існує.", "Name"));

        var slug = model.Slug.Trim().ToLower().Replace(" ", "-");
        var existingBySlug = await context.Categories
            .SingleOrDefaultAsync(x => x.Slug == slug);
        if (existingBySlug != null)
            errors.Add(ErrorHelper.FieldError("Категорія з таким слагом вже існує.", "Slug"));

        if (errors.Any())
            return result.WithErrors(errors);

        model.Slug = slug;
        var entity = mapper.Map<CategoryEntity>(model);

        if (model.ImageFile != null)
            entity.Image = await imageService.SaveImageAsync(model.ImageFile);

        await context.Categories.AddAsync(entity);
        await context.SaveChangesAsync();

        return result.WithValue(entity);
    }

    public async Task<Result<CategoryEntity>> UpdateAsync(CategoryEditModel model)
    {
        var result = new Result<CategoryEntity>();
        var errors = new List<Error>();

        var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (existing == null)
            return result.WithError(ErrorHelper.FieldError("Категорію не знайдено.", "Id"));

        model.Slug = model.Slug.Trim().ToLower().Replace(" ", "-");

        var duplicateName = await context.Categories
            .FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
        if (duplicateName != null)
            errors.Add(ErrorHelper.FieldError("Інша категорія з таким іменем вже існує.", "Name"));

        var duplicateSlug = await context.Categories
            .FirstOrDefaultAsync(x => x.Slug == model.Slug && x.Id != model.Id);
        if (duplicateSlug != null)
            errors.Add(ErrorHelper.FieldError("Інша категорія з таким слагом вже існує.", "Slug"));

        if (errors.Any())
            return result.WithErrors(errors);

        existing = mapper.Map(model, existing);

        if (model.ImageFile != null)
        {
            await imageService.DeleteImageAsync(existing.Image);
            existing.Image = await imageService.SaveImageAsync(model.ImageFile);
        }

        await context.SaveChangesAsync();
        return result.WithValue(existing);
    }

    public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
    {
        var list = await context.Categories.ToListAsync();
        return list;
    }

    public async Task<Result<CategoryEntity>> GetByIdAsync(int id)
    {
        var entity = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        return entity == null
            ? Result.Fail("Категорію не знайдено.")
            : Result.Ok(entity);
    }

    public async Task<Result<CategoryEntity>> GetBySlugAsync(string slug)
    {
        var entity = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == slug);

        return entity == null
            ? Result.Fail("Категорію за вказаним слагом не знайдено.")
            : Result.Ok(entity);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var result = new Result();
        var entity = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
            return result.WithError("Категорію не знайдено для видалення.");

        if (!string.IsNullOrEmpty(entity.Image))
        {
            await imageService.DeleteImageAsync(entity.Image);
        }

        context.Categories.Remove(entity);
        await context.SaveChangesAsync();

        return result;
    }
}
