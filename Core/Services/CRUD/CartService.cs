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

        var cart = await context.Carts
            .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
            throw new Exception("Cart not found.");

        var existingItem = cart.CartItems
            .FirstOrDefault(i => i.ProductId == model.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity = model.Quantity;
            context.CartItems.Update(existingItem);
        }
        else
        {
            var newItem = new CartItemEntity
            {
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                CartId = cart.Id
            };
            context.CartItems.Add(newItem);
        }

        await context.SaveChangesAsync();
    }


    public async Task<CartListModel> GetCartAsync()
    {
        var userId = await authService.GetUserId();

        var model = await context.Carts
            .Where(x => x.UserId == userId)
            .ProjectTo<CartListModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        model.TotalPrice = model.Items
            .Sum(x => x.Quantity * x.Price);

        return model;
    }

    public async Task RemoveCartItemAsync(long cartItemId)
    {
        var entity = await context.CartItems
            .Include(x => x.Cart)
            .FirstOrDefaultAsync(x => x.Id == cartItemId);
        entity!.IsDeleted = true;

        context.CartItems.Update(entity);
        context.SaveChanges();

    }
}
