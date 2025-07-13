using Core.Models.Order;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<List<OrderModel>> GetOrdersAsync();
    Task<long> CreateOrderFromCart(OrderCreateModel model);
    Task<OrderModel> GetOrderByIdAsync(long orderId);
}
