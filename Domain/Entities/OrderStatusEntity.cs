using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("tblOrderStatusEntity")]
public class OrderStatusEntity : BaseEntity<long>
{
    [StringLength(250)]
    public string Name { get; set; } = string.Empty;

    public ICollection<OrderEntity>? Orders { get; set; }
}
