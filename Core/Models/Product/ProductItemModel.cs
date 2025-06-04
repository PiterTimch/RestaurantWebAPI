using Core.Models.Category;
using Core.Models.Ingredient;
using Core.Models.ProductImage;
using Core.Models.ProductSize;

namespace Core.Models.Product;

public class ProductItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int Weight { get; set; }
    public decimal Price { get; set; }
    public CategoryItemModel? Category { get; set; }
    public ProductSizeItemModel? ProductSize { get; set; }
    public ICollection<ProductImageItemModel>? ProductImages { get; set; }
    public ICollection<IngredientItemModel>? ProductIngredients { get; set; }
    public ICollection<ProductVariantItemModel>? Variants { get; set; }
}
