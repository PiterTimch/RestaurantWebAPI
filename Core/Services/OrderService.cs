using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Delivery;
using Core.Models.Order;
using Domain;
using Domain.Entities;
using Domain.Entities.Delivery;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class OrderService(IAuthService authService, 
    AppDbRestaurantContext context, 
    IMapper mapper,
    UserManager<UserEntity> userManager) : IOrderService
{
    public async Task CreateOrder(DeliveryInfoCreateModel model)
    {
        var user = await userManager.FindByIdAsync((await authService.GetUserId()).ToString());

        if (user != null && user.Carts != null)
        {
            var order = new OrderEntity
            {
                UserId = user.Id,
                OrderStatusId = 1
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            var orderItems = user.Carts.Select(item =>
            {
                var oi = mapper.Map<OrderItemEntity>(item);
                oi.OrderId = order.Id;
                return oi;
            }).ToList();

            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();


            await AddDeliveryInfoToOrder(model);
        }
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

    public async Task<List<OrderModel>> GetOrdersAsync()
    {
        var userId = await authService.GetUserId();

        var orderModelList = await context.Orders
            .Where(x => x.UserId == userId)
            .ProjectTo<OrderModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return orderModelList;
    }

    public async Task AddDeliveryInfoToOrder(DeliveryInfoCreateModel model)
    {
        var deliveryInfo = mapper.Map<DeliveryInfoEntity>(model);

        context.DeliveryInfos.Add(deliveryInfo);

        var order = context.Orders
            .FirstOrDefault(x => x.Id == model.OrderId);

        if (order != null)
            order.OrderStatus = context.OrderStatuses
            .FirstOrDefault(x => x.Name == "В обробці");

        await context.SaveChangesAsync();
    }

}
