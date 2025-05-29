using Domain.Entities;
using Core.Models.Category;

namespace Core.Interfaces;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryItemModel>> GetAllAsync();
    Task<CategoryItemModel> GetByIdAsync(int id);
    Task<CategoryItemModel> GetBySlugAsync(string slug);
    Task<CategoryItemModel> CreateAsync(CategoryCreateModel model);
    Task<CategoryItemModel> UpdateAsync(CategoryEditModel model);
    Task DeleteAsync(CategoryDeleteModel model);
}
