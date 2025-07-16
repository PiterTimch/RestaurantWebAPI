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
        public async Task<IActionResult> CreateOrder([FromBody] DeliveryInfoCreateModel model)
        {
            await orderService.CreateOrder(model);
            return Ok();
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
    }
}
