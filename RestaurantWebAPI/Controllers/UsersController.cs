using Core.Interfaces;
using Core.Models.Account;
using Core.Models.Search.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantWebAPI.Constants;

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
    }
}
