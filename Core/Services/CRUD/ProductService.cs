using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Ingredient;
using Core.Models.Product;
using Core.Models.ProductSize;
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
        context.Products.Add(entity);
        await context.SaveChangesAsync();
        foreach (var ingId in model.IngredientIds!)
        {
            var productIngredient = new ProductIngredientEntity
            {
                ProductId = entity.Id,
                IngredientId = ingId
            };
            context.ProductIngredients.Add(productIngredient);
        }
        await context.SaveChangesAsync();


        for (short i = 0; i < model.ImageFiles!.Count; i++)
        {
            try
            {
                var productImage = new ProductImageEntity
                {
                    ProductId = entity.Id,
                    Name = await imageService.SaveImageAsync(model.ImageFiles[i]),
                    Priority = i
                };
                context.ProductImages.Add(productImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Json Parse Data for PRODUCT IMAGE", ex.Message);
            }
        }
        await context.SaveChangesAsync();
        
        var itemModel = mapper.Map<ProductItemModel>(entity);

        return itemModel;
    }

    public async Task<IEnumerable<ProductSizeItemModel>> GetSizesAsync()
    {
        var sizes = await context.ProductSizes
            .ProjectTo<ProductSizeItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        return sizes;
    }

    public async Task<IEnumerable<IngredientItemModel>> GetIngredientsAsync()
    {
        var ingredients = await context.Ingredients
            .ProjectTo<IngredientItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();
        return ingredients;
    }

    public async Task<string> DeleteAsync(ProductDeleteModel model)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == model.Id);
        if (product == null)
            throw new Exception("Продукт не знайдено");

        var variants = await context.Products
            .Where(p => p.ParentProductId == model.Id)
            .Select(p => new ProductDeleteModel() { Id = model.Id })
            .ToListAsync();

        foreach (var variantId in variants)
        {
            await DeleteAsync(variantId);
        }

        var productImages = await context.ProductImages
            .Where(img => img.ProductId == model.Id)
            .ToListAsync();

        if (productImages.Any())
        {
            var imageNames = productImages.Select(img => img.Name).ToList();
            await DeleteImagesAsync(imageNames);

            context.ProductImages.RemoveRange(productImages);
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return $"Продукт {product.Name} видалено";
    }


    private async Task DeleteImagesAsync(List<string> imageNames) 
    {
        imageNames.ForEach(async name => await imageService.DeleteImageAsync(name));
    }

    public async Task<ProductItemModel> UpdateAsync(ProductEditModel model)
    {
        var product = await context.Products
            .Where(p => p.Id == model.Id)
            .ProjectTo<ProductEditModel>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync();


        var entity = await context.Products.FindAsync(model.Id);

        mapper.Map(model, entity);

        var currentIngredients = await context.ProductIngredients
            .Where(x => x.ProductId == model.Id)
            .ToListAsync();

        var newIngredientIds = model.IngredientIds ?? new List<long>();

        var toRemove = currentIngredients
            .Where(x => !newIngredientIds.Contains(x.IngredientId))
            .ToList();

        var toAdd = newIngredientIds
            .Where(id => !currentIngredients.Any(x => x.IngredientId == id))
            .Select(id => new ProductIngredientEntity
            {
                ProductId = model.Id,
                IngredientId = id
            }).ToList();

        if (toRemove.Any())
            context.ProductIngredients.RemoveRange(toRemove);

        if (toAdd.Any())
            await context.ProductIngredients.AddRangeAsync(toAdd);

        var existingImageIds = model.ExistingImageIds ?? new List<long>();

        if (existingImageIds.Any())
        {
            var imagesToUpdate = await context.ProductImages
                .Where(x => existingImageIds.Contains(x.Id))
                .ToListAsync();

            for (short i = 0; i < existingImageIds.Count; i++)
            {
                var img = imagesToUpdate.FirstOrDefault(x => x.Id == existingImageIds[i]);
                if (img != null)
                    img.Priority = i;
            }
        }

        var existingImages = await context.ProductImages
            .Where(x => x.ProductId == model.Id)
            .ToListAsync();

        var imagesToRemove = existingImages
            .Where(x => !existingImageIds.Contains(x.Id))
            .ToList();

        if (imagesToRemove.Any())
        {
            await DeleteImagesAsync(imagesToRemove.Select(x => x.Name).ToList());
            context.ProductImages.RemoveRange(imagesToRemove);
        }

        if (model.NewImages?.Any() == true)
        {
            for (short i = 0; i < model.NewImages.Count; i++)
            {
                try
                {
                    var productImage = new ProductImageEntity
                    {
                        ProductId = model.Id,
                        Name = await imageService.SaveImageAsync(model.NewImages[i]),
                        Priority = i
                    };
                    await context.ProductImages.AddAsync(productImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving product image: " + ex.Message);
                }
            }
        }

        await context.SaveChangesAsync();

        var result = await context.Products
            .Where(p => p.Id == model.Id)
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .FirstAsync();

        return result;
    }

}
