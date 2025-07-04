using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Constants;
using Core.Interfaces;
using Core.Models.Category;
using Core.Models.Search.Params;

namespace RestaurantWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoriesService categoriesService) : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var model = await categoriesService.GetAllAsync();

            return Ok(model);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] CategorySearchModel model)
        {
            var result = await categoriesService.GetAllAsync(model);
            
            return Ok(result);
        }

        [HttpPost("create")]
        //[Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromForm] CategoryCreateModel model)
        {
            var result = await categoriesService.CreateAsync(model);

            return Ok(result);
        }

        [HttpPut("update")]
        //[Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update([FromForm] CategoryEditModel model) 
        {
            var result = await categoriesService.UpdateAsync(model);

            return Ok(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var result = await categoriesService.GetBySlugAsync(slug);

            return Ok(result);
        }

        [HttpDelete("delete")]
        //[Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete([FromBody] CategoryDeleteModel model)
        {
            await categoriesService.DeleteAsync(model);
            return Ok($"Category with id: {model.Id} deleted");
        }
    }
}
