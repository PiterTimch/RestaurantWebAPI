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
    public class CartController(ICartService cartService,
        AppDbRestaurantContext context) : Controller
    {
        [Authorize]
        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart()
        {
            var model = await cartService.GetCartItems();

            return Ok(model);
        }

        [Authorize]
        [HttpPost("createUpdate")]
        public async Task<IActionResult> CreateUpdate([FromBody] CartItemCreateModel model)
        {
            await cartService.CreateUpdate(model);
            
            return Ok();
        }

        [Authorize]
        [HttpPost("add-range")]
        public async Task<IActionResult> AddRange([FromBody] List<CartItemCreateModel> modelItems)
        {
            foreach (var item in modelItems)
            {
                await cartService.CreateUpdate(item);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCartItem(long id)
        {
            await cartService.Delete(id);
            return Ok();
        }
    }
}
