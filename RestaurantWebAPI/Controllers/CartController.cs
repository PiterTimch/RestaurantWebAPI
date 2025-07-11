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
            await cartService.CreateUpdate(model);
            
            return Ok();
        }

        [Authorize]
        [HttpPut("removeCartItem/{productId}")]
        public async Task<IActionResult> RemoveCartItem(long productId)
        {
            await cartService.RemoveCartItemAsync(productId);

            return Ok();
        }
    }
}
