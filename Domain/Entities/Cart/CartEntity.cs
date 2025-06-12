using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Cart;

[Table("tblCarts")]
public class CartEntity : BaseEntity<long>
{
    [ForeignKey("User")]
    public long UserId { get; set; }
    public UserEntity User { get; set; }

    public ICollection<CartItemEntity> CartItems { get; set; } = new List<CartItemEntity>();

}
