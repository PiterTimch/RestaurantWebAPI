using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Interfaces;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryItemModel>> GetAllAsync();
    Task<CategoryItemModel> GetByIdAsync(int id);
    Task<CategoryItemModel> GetBySlugAsync(string slug);
    Task<CategoryItemModel> CreateAsync(CategoryCreateModel model);
    Task<CategoryItemModel> UpdateAsync(CategoryEditModel model);
    Task DeleteAsync(CategoryDeleteModel model);
}
