using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Product;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.CRUD;

public class ProductService(IMapper mapper, AppDbRestaurantContext context) : IProductService
{
    public async Task<IEnumerable<ProductItemModel>> GetAllAsync()
    {
        var model = await context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductSize)
            .Include(p => p.ProductImages)
            .Include(p => p.ProductIngredients)
                .ThenInclude(pi => pi.Ingredient)
            .ToListAsync();

        var models = mapper.Map<List<ProductItemModel>>(model);

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
        var entity = await context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductSize)
            .Include(p => p.ProductImages)
            .Include(p => p.ProductIngredients)
                .ThenInclude(pi => pi.Ingredient)
            .FirstOrDefaultAsync(x => x.Slug == slug);

        var model = mapper.Map<ProductItemModel>(entity);

        return model;
    }
}
