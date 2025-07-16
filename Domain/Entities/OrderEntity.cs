using Domain.Entities.Delivery;
using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("tblOrderEntity")]
public class OrderEntity : BaseEntity<long>
{
    [ForeignKey(nameof(OrderStatus))]
    public long OrderStatusId { get; set; }
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    public OrderStatusEntity? OrderStatus { get; set; }
    public UserEntity? User { get; set; }

    public ICollection<OrderItemEntity>? OrderItems { get; set; }

    public DeliveryInfoEntity? DeliveryInfo { get; set; }
}