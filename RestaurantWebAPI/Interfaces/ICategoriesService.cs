using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Interfaces;

public interface ICategoriesService 
{
    Task<IEnumerable<CategoryItemModel>> GetAllAsync();
    Task<CategoryEntity> GetByIdAsync(int id);
    Task<CategoryEntity> GetBySlugAsync(string slug);
    Task<CategoryEntity> CreateAsync(CategoryCreateModel model);
    Task<CategoryEntity> UpdateAsync(CategoryEditModel model);
    Task DeleteAsync(int id);
}
