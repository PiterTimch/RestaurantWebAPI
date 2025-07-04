using Core.Interfaces;
using Core.Models.Account;
using Core.Models.Search.Params;
using Core.Models.Seeder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Constants;

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

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] UserSearchModel model)
        {
            var result = await userService.SearchUsersAsync(model);
            return Ok(result);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedUsers([FromQuery] SeedItemsModel model)
        {
            string res = await userService.SeedAsync(model);
            return Ok(res);
        }
    }
}
