using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantWebAPI.Data.Entities.Identity;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Account;
using RestaurantWebAPI.Services;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IJWTTokenService tokenService,
        UserManager<UserEntity> userManager) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await tokenService.CreateTokenAsync(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid email or password");
        }
    }
}
