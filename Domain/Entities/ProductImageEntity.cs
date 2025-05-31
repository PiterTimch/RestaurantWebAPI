using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("tblProductImage")]
public class ProductImageEntity : BaseEntity<long>
{
    [StringLength(250)]
    public string Name { get; set; } = String.Empty;

    public short Priority { get; set; }

    [ForeignKey("Product")]
    public long ProductId { get; set; }
    public virtual ProductEntity? Product { get; set; }

}
