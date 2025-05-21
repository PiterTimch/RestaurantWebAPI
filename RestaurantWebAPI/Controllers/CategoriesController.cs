using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Category;

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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateModel model)
        {
            var result = await categoriesService.CreateAsync(model);

            if (result == null)
            {
                return BadRequest($"{model.Name} already exists");
            }

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] CategoryEditModel model) 
        {
            var result = await categoriesService.UpdateAsync(model);

            if (result == null)
            {
                return BadRequest("Invalid update");
            }

            return Ok(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var category = await categoriesService.GetBySlugAsync(slug);

            if (category == null)
            {
                return NotFound($"Invalid category with slug: {slug}");
            }

            return Ok(category);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoriesService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Invalid category with id: {id}");
            }
            await categoriesService.DeleteAsync(id);
            return Ok($"Category with id: {id} deleted");
        }

    }
}
