using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Delivery;
using Core.Models.Order;
using Core.Models.Search.Params;
using Core.Models.Smtp;
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
    ISmtpService smtpService) : IOrderService
{
    public async Task CreateOrder(DeliveryInfoCreateModel model)
    {
        var userId = (await authService.GetUserId()).ToString();
        var user = await context.Users
            .Include(u => u.Carts)
            .ThenInclude(c => c.Product)
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

            var totalPrice = orderItems.Sum(i => i.Count * i.PriceBuy);

            var htmlBody = $@"
                <div style='font-family: Arial, sans-serif; background-color: #fff7f0; padding: 20px; color: #333;'>
                    <h2 style='color: #d35400;'>Дякуємо за ваше замовлення!</h2>
                    <p>Ваше замовлення №<strong>{order.Id}</strong> успішно створено та зараз перебуває в обробці.</p>
                    <h3 style='color: #e67e22;'>Деталі замовлення:</h3>
                    <table style='width:100%; border-collapse: collapse; background-color: #fff; border: 1px solid #e67e22;'>
                        <thead>
                            <tr style='background-color: #fbe6d4; color: #d35400;'>
                                <th style='border: 1px solid #e67e22; padding: 10px;'>Назва товару</th>
                                <th style='border: 1px solid #e67e22; padding: 10px;'>Ціна</th>
                                <th style='border: 1px solid #e67e22; padding: 10px;'>Кількість</th>
                                <th style='border: 1px solid #e67e22; padding: 10px;'>Сума</th>
                            </tr>
                        </thead>
                        <tbody>
            ";

            foreach (var item in orderItems)
            {
                var productName = item.Product?.Name ?? "Товар";
                var itemPrice = item.PriceBuy.ToString("0.00");
                var itemTotal = (item.PriceBuy * item.Count).ToString("0.00");

                htmlBody += $@"
                    <tr style='background-color: #fffaf5;'>
                        <td style='border: 1px solid #f0caa3; padding: 10px;'>{productName}</td>
                        <td style='border: 1px solid #f0caa3; padding: 10px;'>{itemPrice} грн</td>
                        <td style='border: 1px solid #f0caa3; padding: 10px;'>{item.Count}</td>
                        <td style='border: 1px solid #f0caa3; padding: 10px;'>{itemTotal} грн</td>
                    </tr>
                ";
            }

            htmlBody += $@"
                        </tbody>
                    </table>

                    <p style='margin-top: 20px; font-size: 16px;'><strong>Загальна сума: <span style='color: #d35400;'>{totalPrice.ToString("0.00")} грн</span></strong></p>
                    <br/>
                    <p>Ми повідомимо вас, коли замовлення буде відправлено.</p>
                    <p style='font-size: 12px; color: gray;'>Цей лист згенеровано автоматично.</p>
                </div>";


            await smtpService.SendEmailAsync(new EmailMessage
            {
                To = user.Email!,
                Subject = $"Підтвердження замовлення №{order.Id}",
                Body = htmlBody
            });

        }
        else
        {
            throw new InvalidOperationException("Користувач не знайдений або кошик порожній.");
        }
    }

    public async Task<List<CityModel>> GetCities(CitySearchModel model)
    {
        var query = context.Cities.AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            var search = model.Name.Trim().ToLower();

            query = query.Where(c =>
                c.Name.ToLower() == search ||
                c.Name.ToLower().StartsWith(search) ||
                c.Name.ToLower().Contains(" " + search)
            );

            query = query.OrderBy(c =>
                c.Name.ToLower() == search ? 0 :
                c.Name.ToLower().StartsWith(search) ? 1 :
                c.Name.ToLower().Contains(" " + search) ? 2 : 3
            );
        }

        var cities = await query
            .ProjectTo<CityModel>(mapper.ConfigurationProvider)
            .Take(model.ItemPerPage)
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

    public Task<List<PostDepartmentModel>> GetPostDepartments(PostDepartmentSearchModel model)
    {
        var query = context.PostDepartments.AsQueryable();

        if (!string.IsNullOrEmpty(model.CityName))
        {
            query = query.Where(pd => pd.City!.Name.ToLower().Contains(model.CityName.ToLower()));
        }

        var postDepartments = query
            .Include(pd => pd.City)
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

    public async Task<string> GetLastOrderAddress()
    {
        var userId = await authService.GetUserId();

        string? lastAddress = await context.DeliveryInfos
            .Where(x => x.Order.UserId == userId)
            .OrderByDescending(x => x.Order.DateCreated)
            .Select(x => $"{x.PostDepartment.Name} {x.PostDepartment.City.Name}")
            .FirstOrDefaultAsync();

        return lastAddress ?? "-";
    }
}
