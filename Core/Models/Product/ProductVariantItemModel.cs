using Core.Models.ProductImage;
using Core.Models.ProductSize;

namespace Core.Models.Product;

public class ProductVariantItemModel
{
    public long Id { get; set; }
    public int Weight { get; set; }
    public decimal Price { get; set; }

    public ProductSizeItemModel? ProductSize { get; set; }
    public ICollection<ProductImageItemModel>? ProductImages { get; set; }
}
