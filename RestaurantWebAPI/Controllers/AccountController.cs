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
    public class AccountController(IAccountService accountService) : Controller
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            string result = await accountService.LoginAsync(model);
            return Ok(new
            {
                Token = result
            });
        }
    }
}
