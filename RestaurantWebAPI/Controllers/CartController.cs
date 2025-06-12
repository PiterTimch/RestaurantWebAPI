using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Account;
using Core.Services;
using Core.Models.Cart;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService, AppDbRestaurantContext context) : Controller
    {
        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart(long userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user ID.");
            if (!await context.Users.AnyAsync(u => u.Id == userId))
                return NotFound("User not found.");

            var model = await cartService.GetCartAsync(userId);

            return Ok(model);
        }

        [HttpPost("addCartItem")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemCreateModel model)
        {
            var cartListModel = await cartService.AddCartItemAsync(model);
            return Ok(cartListModel);
        }

        [HttpPut("updateCartItemQuantity")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] CartItemQuantityEditModel model)
        {
            var cartListModel = await cartService.UpdateCartItemQuantityAsync(model);
            return Ok(cartListModel);
        }

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
