using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Account;
using Core.Services;
using Core.Models.Cart;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Core.Models.Order;
using Core.Models.Delivery;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService) : Controller
    {
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetUserOrders()
        {
            var model = await orderService.GetOrdersAsync();

            return Ok(model);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] OrderCreateModel model)
        {
            if (model.CartId <= 0)
            {
                return BadRequest("Invalid cart ID.");
            }
            var orderId = await orderService.CreateOrderFromCart(model);
            if (orderId > 0)
            {
                return Ok(new { OrderId = orderId });
            }
            return BadRequest("Failed to create order from cart.");
        }

        [Authorize]
        [HttpGet("get/{orderId}")]
        public async Task<IActionResult> GetOrderById(long orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("Invalid order ID.");
            }
            var order = await orderService.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                return Ok(order);
            }
            return NotFound("Order not found.");
        }

        [Authorize]
        [HttpGet("cities")]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await orderService.GetAllCities();
            return Ok(cities);
        }

        [Authorize]
        [HttpGet("post-departments")]
        public async Task<IActionResult> GetAllPostDepartments()
        {
            var postDepartments = await orderService.GetAllPostDepartments();
            return Ok(postDepartments);
        }

        [Authorize]
        [HttpGet("payment-types")]
        public async Task<IActionResult> GetAllPaymentTypes()
        {
            var paymentTypes = await orderService.GetAllPaynamentTypes();
            return Ok(paymentTypes);
        }

        [Authorize]
        [HttpPost("add-delivery-info")]
        public async Task<IActionResult> AddDeliveryInfoToOrder([FromBody] DeliveryInfoCreateModel model)
        {
            if (model == null || model.OrderId <= 0)
            {
                return BadRequest("Invalid delivery information.");
            }
            await orderService.AddDeliveryInfoToOrder(model);
            return Ok();
        }
    }
}
