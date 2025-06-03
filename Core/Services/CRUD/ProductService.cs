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
        var models = await context.Products
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
}
