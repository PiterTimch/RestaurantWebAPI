using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(AppDbRestaurantContext context, 
        IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await mapper.ProjectTo<CategoryItemModel>(context.Categories).ToListAsync();
            return Ok(model);
        }
    }
}
