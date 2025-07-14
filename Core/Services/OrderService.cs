using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Delivery;
using Core.Models.Order;
using Domain;
using Domain.Entities;
using Domain.Entities.Delivery;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class OrderService(IAuthService authService, AppDbRestaurantContext context, IMapper mapper) : IOrderService
{
    public Task AddDeliveryInfoToOrder(DeliveryInfoCreateModel model)
    {
        var deliveryInfo = mapper.Map<DeliveryInfoEntity>(model);

        context.DeliveryInfos.Add(deliveryInfo);
        return context.SaveChangesAsync();
    }

    public async Task<long> CreateOrderFromCart(OrderCreateModel model)
    {
        var cart = await context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(x => x.Id == model.CartId);

        if (cart != null)
        {
            var order = new OrderEntity
            {
                UserId = cart.UserId,
                OrderStatusId = 1
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            var orderItems = cart.CartItems.Select(item =>
            {
                var oi = mapper.Map<OrderItemEntity>(item);
                oi.OrderId = order.Id;
                return oi;
            }).ToList();

            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();

            return order.Id;
        }

        return 0;
    }

    public Task<List<CityModel>> GetAllCities()
    {
        var cities = context.Cities
            .ProjectTo<CityModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return cities;
    }

    public Task<List<PaynamentTypeModel>> GetAllPaynamentTypes()
    {
        var paymentTypes = context.PaymentTypes
            .ProjectTo<PaynamentTypeModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return paymentTypes;
    }

    public Task<List<PostDepartmentModel>> GetAllPostDepartments()
    {
        var postDepartments = context.PostDepartments
            .ProjectTo<PostDepartmentModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return postDepartments;
    }

    public async Task<OrderModel> GetOrderByIdAsync(long orderId)
    {
        var userId = await authService.GetUserId();

        var result = await context.Orders
            .Where(x => x.Id == orderId && x.UserId == userId)
            .ProjectTo<OrderModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<List<OrderModel>> GetOrdersAsync()
    {
        var userId = await authService.GetUserId();

        var orderModelList = await context.Orders
            .Where(x => x.UserId == userId)
            .ProjectTo<OrderModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return orderModelList;
    }

}
