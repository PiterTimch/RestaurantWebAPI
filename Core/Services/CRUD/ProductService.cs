using Core.Interfaces;
using Core.Models.Product;

namespace Core.Services.CRUD;

public class ProductService() : IProductService
{
    public Task<IEnumerable<ProductItemModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ProductItemModel> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductItemModel> GetBySlugAsync(string slug)
    {
        throw new NotImplementedException();
    }
}
