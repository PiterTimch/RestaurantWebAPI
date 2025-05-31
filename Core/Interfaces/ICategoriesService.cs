using Domain.Entities;
using Core.Models.Category;
using Core.Models.Search;
using Core.Models.Common;

namespace Core.Interfaces;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryItemModel>> GetAllAsync();
    Task<PaginationModel<CategoryItemModel>> GetAllAsync(CategorySearchModel searchModel);
    Task<CategoryItemModel> GetByIdAsync(int id);
    Task<CategoryItemModel> GetBySlugAsync(string slug);
    Task<CategoryItemModel> CreateAsync(CategoryCreateModel model);
    Task<CategoryItemModel> UpdateAsync(CategoryEditModel model);
    Task DeleteAsync(CategoryDeleteModel model);
}
