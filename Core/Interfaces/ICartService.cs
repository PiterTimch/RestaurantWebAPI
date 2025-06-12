using Core.Models.Cart;

namespace Core.Interfaces;

public interface ICartService
{
    Task<CartListModel> GetCartAsync(long userId);
    Task<CartListModel> AddCartItemAsync(CartItemCreateModel model);
    Task<CartListModel> UpdateCartItemQuantityAsync(CartItemQuantityEditModel model);
    Task<CartListModel> RemoveCartItemAsync(long cartItemId);
}
