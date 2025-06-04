using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("tblProducts")]
public class ProductEntity : BaseEntity<long>
{
    [StringLength(250)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string Slug { get; set; } = string.Empty;

    public int Weight { get; set; }

    public decimal Price { get; set; }

    [ForeignKey("Category")]
    public long CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }

    [ForeignKey("ProductSize")]
    public long? ProductSizeId { get; set; }
    public ProductSizeEntity? ProductSize { get; set; }

    public long? ParentProductId { get; set; }

    [ForeignKey("ParentProductId")]
    public ProductEntity? ParentProduct { get; set; }

    public ICollection<ProductEntity>? Variants { get; set; }

    public ICollection<ProductIngredientEntity>? ProductIngredients { get; set; }
    public ICollection<ProductImageEntity>? ProductImages { get; set; }
}
