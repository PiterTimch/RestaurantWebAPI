using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantWebAPI.Constants;
using Core.Interfaces;
using Core.Models.Category;
using Core.Models.Search;
using Core.Models.Product;

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

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetBySlug(int id)
        {
            var result = await productService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ProductCreateModel model)
        {
            var createdProduct = await productService.CreateAsync(model);

            return Ok(createdProduct);
        }
    }
}
