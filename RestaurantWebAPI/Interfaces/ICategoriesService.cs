using FluentResults;
using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Interfaces;

public interface ICategoriesService 
{
    Task<IEnumerable<CategoryEntity>> GetAllAsync();
    Task<Result<CategoryEntity>> GetByIdAsync(int id);
    Task<Result<CategoryEntity>> GetBySlugAsync(string slug);
    Task<Result<CategoryEntity>> CreateAsync(CategoryCreateModel model);
    Task<Result<CategoryEntity>> UpdateAsync(CategoryEditModel model);
    Task<Result> DeleteAsync(int id);
}
