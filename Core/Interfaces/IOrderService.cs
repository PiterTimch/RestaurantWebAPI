using Core.Models.Delivery;
using Core.Models.Order;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<List<OrderModel>> GetOrdersAsync();
    Task CreateOrder(DeliveryInfoCreateModel model);
    Task<List<CityModel>> GetAllCities();
    Task<List<PostDepartmentModel>> GetAllPostDepartments();
    Task<List<PaynamentTypeModel>> GetAllPaynamentTypes();
}
