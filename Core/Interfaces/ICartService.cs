using Core.Models.Cart;

namespace Core.Interfaces;

public interface ICartService
{
    Task<CartListModel> GetCartAsync();
    Task CreateUpdate(CartItemCreateModel model);
    Task RemoveCartItemAsync(long cartItemId);
}
