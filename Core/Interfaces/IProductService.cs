using Core.Models.Ingredient;
using Core.Models.Product;
using Core.Models.ProductSize;
using Core.Models.Search;
using Core.Models.Search.Params;

namespace Core.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductItemModel>> GetAllAsync();
    Task<ProductItemModel> GetByIdAsync(long id);
    Task<ProductItemModel> GetBySlugAsync(string slug);
    Task<ProductItemModel> CreateAsync(ProductCreateModel model);
    Task<IEnumerable<IngredientItemModel>> GetIngredientsAsync();
    Task<IEnumerable<ProductSizeItemModel>> GetSizesAsync();
    Task<string> DeleteAsync(ProductDeleteModel model);
    Task<ProductItemModel> UpdateAsync(ProductEditModel model);
    Task<IngredientItemModel> UploadIngredient(IngredientCreateModel model);
    Task<SearchResult<ProductItemModel>> SearchProductsAsync(ProductSearchModel model);
}
