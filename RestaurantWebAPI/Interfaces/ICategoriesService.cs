using FluentResults;
using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Interfaces;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryItemModel>> GetAllAsync();
    Task<Result<CategoryItemModel>> GetByIdAsync(int id);
    Task<Result<CategoryItemModel>> GetBySlugAsync(string slug);
    Task<Result<CategoryItemModel>> CreateAsync(CategoryCreateModel model);
    Task<Result<CategoryItemModel>> UpdateAsync(CategoryEditModel model);
    Task<Result> DeleteAsync(int id);
}
