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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await categoriesService.CreateAsync(model);

            if (result == null)
            {
                return BadRequest("Category already exists");
            }

            return Ok(result);
        }
    }
}
