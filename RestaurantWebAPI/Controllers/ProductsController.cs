using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantWebAPI.Constants;
using Core.Interfaces;
using Core.Models.Category;
using Core.Models.Search;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var model = await productService.GetAllAsync();

            return Ok(model);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var result = await productService.GetBySlugAsync(slug);

            return Ok(result);
        }
    }
}
