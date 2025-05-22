using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Category;
using RestaurantWebAPI.Validators.Helpers;

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

            if (result.IsFailed)
                return BadRequest(result.Errors.ToFieldErrors());

            return Ok(result.Value);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] CategoryEditModel model) 
        {
            var result = await categoriesService.UpdateAsync(model);

            if (result.IsFailed)
                return BadRequest(result.Errors.ToFieldErrors());

            return Ok(result.Value);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var result = await categoriesService.GetBySlugAsync(slug);

            if (result.IsFailed)
                return BadRequest($"Errors: {ErrorsToString(result.Errors)}");

            return Ok(result.Value);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await categoriesService.DeleteAsync(id);

            if (result.IsFailed)
                return BadRequest($"Errors: {ErrorsToString(result.Errors)}");

            return Ok($"Category with id: {id} deleted");
        }

        private string ErrorsToString(IEnumerable<IError> errors)
        {
            return string.Join(", ", errors.Select(e => e.Message));
        }
    }
}
