using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Account;
using Core.Services;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : Controller
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers() 
        {
            var model = await accountService.GetAllUsersAsync();

            return Ok(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            string result = await accountService.LoginAsync(model);
            return Ok(new
            {
                Token = result
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            string result = await accountService.RegisterAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest(new
                {
                    Status = 400,
                    IsValid = false,
                    Errors = new { Email = "Помилка пеєстрації" }
                });
            }
            return Ok(new
            {
                Token = result
            });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel model)
        {
            await accountService.DeleteUserAsync(model);
            return Ok($"Користувача з id {model.Id} успішно видалено");
        }
    }
}
