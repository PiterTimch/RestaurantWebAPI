using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Cart;

[Table("tblCartItems")]
public class CartItemEntity : BaseEntity<long>
{
    [Range(0, 50)]
    public int Quantity { get; set; }

    [ForeignKey("Product")]
    public long ProductId { get; set; }
    public ProductEntity Product { get; set; }

    [ForeignKey("Card")]
    public long CartId { get; set; }
    public CartEntity Cart { get; set; }

}
