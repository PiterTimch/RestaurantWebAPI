using Core.Models.Product;

namespace Core.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductItemModel>> GetAllAsync();
    Task<ProductItemModel> GetByIdAsync(long id);
    Task<ProductItemModel> GetBySlugAsync(string slug);
}
