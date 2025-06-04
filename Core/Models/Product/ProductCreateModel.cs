using Core.Models.Ingredient;
using Core.Models.ProductImage;
using Microsoft.AspNetCore.Http;

namespace Core.Models.Product;

public class ProductCreateModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int Weight { get; set; }
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
    public int? ProductSizeId { get; set; }
    public ICollection<ProductImageCreateModel>? ProductImages { get; set; }
    public ICollection<long>? IngredientIds { get; set; }
    public ICollection<NewIngredientModel>? NewIngredients { get; set; }
}
