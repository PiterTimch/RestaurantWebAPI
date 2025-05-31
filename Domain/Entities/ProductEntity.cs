using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("tblProducts")]
public class ProductEntity : BaseEntity<long>
{
    [StringLength(250)]
    public string Name { get; set; } = String.Empty;

    [StringLength(250)]
    public string Slug { get; set; } = String.Empty;

    [StringLength(200)]
    public string Image { get; set; } = String.Empty;

    public int Weight { get; set; }

    public decimal Price { get; set; }

    [ForeignKey("Category")]
    public long CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }

    [ForeignKey("ProductSize")]
    public long? ProductSizeId { get; set; }
    public ProductSizeEntity? ProductSize { get; set; }

}
