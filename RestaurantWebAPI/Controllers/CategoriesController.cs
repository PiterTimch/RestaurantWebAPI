using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Constants;
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
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CategoryCreateModel model)
        {
            var result = await categoriesService.CreateAsync(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] CategoryDeleteModel model)
        {
            await categoriesService.DeleteAsync(model);
            return Ok($"Category with id: {model.Id} deleted");
        }
    }
}
