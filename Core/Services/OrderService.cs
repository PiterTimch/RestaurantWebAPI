using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Order;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class OrderService(IAuthService authService, AppDbRestaurantContext context, IMapper mapper) : IOrderService
{
    public async Task<List<OrderModel>> GetOrdersAsync()
    {
        var userId = await authService.GetUserId();

        var orderModelList = await context.Orders
            .Where(x => x.UserId == userId)
            .ProjectTo<OrderModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        orderModelList = orderModelList
        .Select(item =>
        {
            item.TotalPrice = item.OrderItems.Sum(oi => oi.PriceBuy * oi.Count);
            return item;
        })
        .ToList();

        return orderModelList;
    }

}
