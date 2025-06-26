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
            .Where(x => !x.IsDeleted)
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
        product!.IsDeleted = true;
        await context.SaveChangesAsync();
        //if (product == null)
        //    throw new Exception("Продукт не знайдено");

        //var variants = await context.Products
        //    .Where(p => p.ParentProductId == model.Id)
        //    .Select(p => new ProductDeleteModel() { Id = model.Id })
        //    .ToListAsync();

        //foreach (var variantId in variants)
        //{
        //    await DeleteAsync(variantId);
        //}

        //var productImages = await context.ProductImages
        //    .Where(img => img.ProductId == model.Id)
        //    .ToListAsync();

        //if (productImages.Any())
        //{
        //    var imageNames = productImages.Select(img => img.Name).ToList();
        //    await DeleteImagesAsync(imageNames);

        //    context.ProductImages.RemoveRange(productImages);
        //}

        //context.Products.Remove(product);
        //await context.SaveChangesAsync();

        return $"Продукт {product.Name} видалено";
    }


    private async Task DeleteImagesAsync(List<string> imageNames) 
    {
        imageNames.ForEach(async name => await imageService.DeleteImageAsync(name));
    }

    public async Task<ProductItemModel> UpdateAsync(ProductEditModel model)
    {
        var entity = await context.Products
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync();

        mapper.Map(model, entity);

        var item = await context.Products
            .Where(x => x.Id == model.Id)
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        var imgDelete = item.ProductImages
            .Where(x => !model.ImageFiles!.Any(y => y.FileName == x.Name))
            .ToList();

        foreach (var img in imgDelete)
        {
            var productImage = await context.ProductImages
                .Where(x => x.Id == img.Id)
                .SingleOrDefaultAsync();
            if (productImage != null)
            {
                await imageService.DeleteImageAsync(productImage.Name);
                context.ProductImages.Remove(productImage);
            }
            context.SaveChanges();
        }

        short p = 0;
        foreach (var imgFile in model.ImageFiles!)
        {
            if (imgFile.ContentType == "old-image")
            {
                var img = await context.ProductImages
                    .Where(x => x.Name == imgFile.FileName)
                    .SingleOrDefaultAsync();
                img.Priority = p;
                context.SaveChanges();
            }

            else
            {
                try
                {
                    var productImage = new ProductImageEntity
                    {
                        ProductId = item.Id,
                        Name = await imageService.SaveImageAsync(imgFile),
                        Priority = p
                    };
                    context.ProductImages.Add(productImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data for PRODUCT IMAGE", ex.Message);
                }
            }

            p++;

        }

        var existingIngredients = context.ProductIngredients
            .Where(pi => pi.ProductId == item.Id);
        context.ProductIngredients.RemoveRange(existingIngredients);

        if (model.IngredientIds != null)
        {
            foreach (var ingredientId in model.IngredientIds.Distinct())
            {
                var newIngredient = new ProductIngredientEntity
                {
                    ProductId = item.Id,
                    IngredientId = ingredientId
                };
                context.ProductIngredients.Add(newIngredient);
            }
        }

        await context.SaveChangesAsync();
        return item;
    }

    public async Task<IngredientItemModel> UploadIngredient(IngredientCreateModel model)
    {
        var entity = mapper.Map<IngredientEntity>(model);
        entity.Image = await imageService.SaveImageAsync(model.ImageFile!);
        context.Ingredients.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<IngredientItemModel>(entity);
    }
}
