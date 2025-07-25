using Core.Models.Delivery;
using Core.Models.Order;
using Core.Models.Search.Params;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<List<OrderModel>> GetOrdersAsync();
    Task CreateOrder(DeliveryInfoCreateModel model);
    Task<List<CityModel>> GetCities(CitySearchModel model);
    Task<List<PostDepartmentModel>> GetPostDepartments(PostDepartmentSearchModel model);
    Task<List<PaynamentTypeModel>> GetAllPaynamentTypes();
    Task<string> GetLastOrderAddress();
}
