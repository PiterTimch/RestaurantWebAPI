using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Cart;
using Domain;
using Domain.Entities.Cart;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.CRUD;

public class CartService(IMapper mapper, AppDbRestaurantContext context) : ICartService
{
    public async Task<CartListModel> AddCartItemAsync(CartItemCreateModel model)
    {
        var cartEntity = await context.Carts
            .Include(c => c.CartItems) // не додумався як замінити :(
            .FirstOrDefaultAsync(x => x.UserId == model.UserId);

        cartEntity!.CartItems.Add(new CartItemEntity
        {
            ProductId = model.ProductId,
            Quantity = 1,
            CartId = cartEntity.Id
        });

        context.Carts.Update(cartEntity);
        context.SaveChanges();

        return await GetCartAsync(model.UserId);
    }

    public async Task<CartListModel> GetCartAsync(long userId)
    {
        var model = await context.Carts
            .Where(x => x.UserId == userId)
            .ProjectTo<CartListModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        model.TotalPrice = model.Items
            .Sum(x => x.Quantity * x.Price);

        return model;
    }

    public async Task<CartListModel> RemoveCartItemAsync(long cartItemId)
    {
        var entity = await context.CartItems
            .Include(x => x.Cart)
            .FirstOrDefaultAsync(x => x.Id == cartItemId);
        entity!.IsDeleted = true;

        context.CartItems.Update(entity);
        context.SaveChanges();

        return await GetCartAsync(entity.Cart.UserId);

    }

    public Task<CartListModel> UpdateCartItemQuantityAsync(CartItemQuantityEditModel model)
    {
        var entity = context.CartItems
            .Include(x => x.Cart)
            .FirstOrDefault(x => x.Id == model.CartItemId);
        entity!.Quantity = model.NewQuantity;

        context.CartItems.Update(entity);
        context.SaveChanges();

        return GetCartAsync(entity.Cart.UserId);
    }
}
