using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Cart;
using Domain;
using Domain.Entities.Cart;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.CRUD;

public class CartService(IMapper mapper, AppDbRestaurantContext context, IAuthService authService) : ICartService
{
    public async Task CreateUpdate(CartItemCreateModel model)
    {
        var userId = await authService.GetUserId();
        var entity = context.Carts
            .SingleOrDefault(x => x.UserId == userId && x.ProductId == model.ProductId);
        if (entity != null)
            entity.Quantity = model.Quantity;
        else
        {
            entity = new CartEntity
            {
                UserId = userId,
                ProductId = model.ProductId,
                Quantity = model.Quantity
            };
            context.Carts.Add(entity);
        }
        await context.SaveChangesAsync();
    }

    public async Task<List<CartItemModel>> GetCartItems()
    {
        var userId = await authService.GetUserId();

        var items = await context.Carts
            .Where(x => x.UserId == userId)
            .ProjectTo<CartItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return items;
    }

    public async Task Delete(long id)
    {
        var userId = await authService.GetUserId();
        var item = await context.Carts
            .SingleOrDefaultAsync(x => x.UserId == userId && x.ProductId == id);
        if (item != null)
        {
            context.Carts.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
