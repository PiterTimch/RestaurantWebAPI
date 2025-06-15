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
    }
}
