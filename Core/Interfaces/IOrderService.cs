using Core.Models.Order;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<List<OrderModel>> GetOrdersAsync();
}
