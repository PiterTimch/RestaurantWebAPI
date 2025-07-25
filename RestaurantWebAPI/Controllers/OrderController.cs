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
using Core.Models.Search.Params;

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
            try
            {
                await orderService.CreateOrder(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("cities")]
        public async Task<IActionResult> GetAllCities([FromQuery] CitySearchModel model)
        {
            if (model.ItemPerPage < 1)
            {
                return BadRequest("ItemPerPage must be greater than 0.");
            }
            var cities = await orderService.GetCities(model);
            return Ok(cities);
        }

        [Authorize]
        [HttpGet("post-departments")]
        public async Task<IActionResult> GetAllPostDepartments([FromQuery] PostDepartmentSearchModel model)
        {
            var pds = await orderService.GetPostDepartments(model);
            return Ok(pds);
        }

        [Authorize]
        [HttpGet("payment-types")]
        public async Task<IActionResult> GetAllPaymentTypes()
        {
            var paymentTypes = await orderService.GetAllPaynamentTypes();
            return Ok(paymentTypes);
        }

        [Authorize]
        [HttpGet("last-order-address")]
        public async Task<IActionResult> GetLastOrderAddress()
        {
            var address = await orderService.GetLastOrderAddress();
            return Ok(address);
        }
    }
}
