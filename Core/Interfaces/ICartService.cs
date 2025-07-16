using Core.Models.Cart;

namespace Core.Interfaces;

public interface ICartService
{
    Task CreateUpdate(CartItemCreateModel model);
    Task<List<CartItemModel>> GetCartItems();
    Task Delete(long id);
}
