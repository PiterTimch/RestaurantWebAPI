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
            var model = await cartService.GetCartAsync();

            return Ok(model);
        }

        [Authorize]
        [HttpPost("createUpdate")]
        public async Task<IActionResult> CreateUpdate([FromBody] CartItemCreateModel model)
        {
            var cartListModel = await cartService.CreateUpdate(model);
            return Ok();
        }

        [Authorize]
        [HttpPut("removeCartItem/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(long cartItemId)
        {
            if (cartItemId <= 0)
                return BadRequest("Invalid cart item ID.");
            if (!await context.CartItems.AnyAsync(ci => ci.Id == cartItemId && !ci.IsDeleted))
                return NotFound("Cart item not found or already deleted.");

            var cartListModel = await cartService.RemoveCartItemAsync(cartItemId);
            return Ok(cartListModel);
        }
    }
}
