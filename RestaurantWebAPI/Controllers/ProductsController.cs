using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Constants;
using Core.Interfaces;
using Core.Models.Category;
using Core.Models.Search;
using Core.Models.Product;
using Core.Models.Ingredient;
using Core.Models.Search.Params;

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

        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizes()
        {
            var sizes = await productService.GetSizesAsync();

            return Ok(sizes);
        }

        [HttpGet("ingredients")]
        public async Task<IActionResult> GetIngredients()
        {
            var ingredients = await productService.GetIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpPost("ingredients")]
        public async Task<IActionResult> CreateIngredient([FromForm] IngredientCreateModel model)
        {
            var ingredient = await productService.UploadIngredient(model);
            if (ingredient != null)
                return Ok(ingredient);
            return BadRequest();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(ProductDeleteModel model)
        {
            var result = await productService.DeleteAsync(model);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ProductEditModel model)
        {
            var updatedProduct = await productService.UpdateAsync(model);
            return Ok(updatedProduct);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ProductSearchModel model)
        {
            var result = await productService.SearchProductsAsync(model);
            return Ok(result);
        }
    }
}
