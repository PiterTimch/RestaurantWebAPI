using Core.Interfaces;
using Core.Models.Account;
using Core.Models.Search.Params;
using Core.Models.Seeder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Constants;
using Core.Validators.User;
using Core.Models.AdminUser;
using System.Diagnostics;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : Controller
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            var model = await userService.GetAllUsersAsync();

            return Ok(model);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchUsers([FromBody] UserSearchModel model)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            var result = await userService.SearchUsersAsync(model);
            
            stopwatch.Stop();

            Console.WriteLine($"Search Users took {stopwatch.ElapsedMilliseconds} ms");

            return Ok(result);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedUsers([FromQuery] SeedItemsModel model)
        {
            string res = await userService.SeedAsync(model);
            return Ok(res);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditUser([FromForm] AdminUserEditModel model)
        {
            var res = await userService.EditUserAsync(model);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with id: {id} not found");
            }
            return Ok(user);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] AdminUserDeleteModel model)
        {
            await userService.DeleteUser(model.Id);
            return Ok();
        }
    }
}
