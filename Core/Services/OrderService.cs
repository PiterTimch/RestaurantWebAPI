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
        var userId = (await authService.GetUserId()).ToString();
        var user = await context.Users
            .Include(u => u.Carts)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (user != null && user.Carts != null && user.Carts.Any())
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


            var deliveryInfo = mapper.Map<DeliveryInfoEntity>(model);
            deliveryInfo.OrderId = order.Id;

            context.DeliveryInfos.Add(deliveryInfo);

            if (order != null)
                order.OrderStatus = context.OrderStatuses
                .FirstOrDefault(x => x.Name == "В обробці");

            user.Carts.Clear();

            await context.SaveChangesAsync();

        }
        else
        {
            throw new InvalidOperationException("Користувач не знайдений або кошик порожній.");
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

}
