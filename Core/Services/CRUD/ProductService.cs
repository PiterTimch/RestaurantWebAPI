using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.Services.CRUD;

public class ProductService(IMapper mapper,
    AppDbRestaurantContext context, IImageService imageService) : IProductService
{

    public async Task<IEnumerable<ProductItemModel>> GetAllAsync()
    {
        var models = await context.Products
            .Where(x => x.ParentProduct == null)
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return models;
    }

    public async Task<ProductItemModel> GetByIdAsync(long id)
    {
        var model = await context.Products
            .Where(p => p.Id == id)
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return model;
    }

    public async Task<ProductItemModel> GetBySlugAsync(string slug)
    {
        var model = await context.Products
            .Where(p => p.Slug == slug)
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return model;
    }

    public async Task<ProductItemModel> CreateAsync(ProductCreateModel model)
    {
        var entity = mapper.Map<ProductEntity>(model);

        entity.ProductImages = model.ProductImages != null
            ? (await Task.WhenAll(model.ProductImages.Select(async image =>
                new ProductImageEntity { Name = await imageService.SaveImageAsync(image.ImageFile) })))
                .ToList()
            : new List<ProductImageEntity>();

        entity.ProductIngredients = new List<ProductIngredientEntity>();

        if (model.IngredientIds != null && model.IngredientIds.Any())
        {
            var existingIngredients = await context.Ingredients
                .Where(i => model.IngredientIds.Contains(i.Id))
                .ToListAsync();

            foreach (var ingredient in existingIngredients)
            {
                entity.ProductIngredients.Add(new ProductIngredientEntity
                {
                    Ingredient = ingredient
                });
            }
        }

        if (model.NewIngredients != null && model.NewIngredients!.Any())
        {
            foreach (var newIngredient in model.NewIngredients!)
            {
                var newIngredientEntity = new IngredientEntity
                {
                    Name = newIngredient.Name,
                    Image = newIngredient.ImageFile != null
                        ? await imageService.SaveImageAsync(newIngredient.ImageFile)
                        : null
                };

                context.Ingredients.Add(newIngredientEntity);

                entity.ProductIngredients.Add(new ProductIngredientEntity
                {
                    Ingredient = newIngredientEntity
                });
            }
        }

        await context.Products.AddAsync(entity);
        context.SaveChanges();

        var itemModel = mapper.Map<ProductItemModel>(entity);
        
        return itemModel;
    }
}
