using Core.Models.Cart;

namespace Core.Interfaces;

public interface ICartService
{
    Task<CartListModel> GetCartAsync();
    Task<CartListModel> CreateUpdate(CartItemCreateModel model);
    Task<CartListModel> RemoveCartItemAsync(long cartItemId);
}
