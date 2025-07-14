using Core.Models.Delivery;
using Core.Models.Order;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<List<OrderModel>> GetOrdersAsync();
    Task<long> CreateOrderFromCart(OrderCreateModel model);
    Task<OrderModel> GetOrderByIdAsync(long orderId);
    Task AddDeliveryInfoToOrder(DeliveryInfoCreateModel model);
    Task<List<CityModel>> GetAllCities();
    Task<List<PostDepartmentModel>> GetAllPostDepartments();
    Task<List<PaynamentTypeModel>> GetAllPaynamentTypes();
}
