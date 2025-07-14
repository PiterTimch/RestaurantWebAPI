using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Delivery;

[Table("tblDeliveryInfos")]
public class DeliveryInfoEntity : BaseEntity<long>
{
    [ForeignKey("City")]
    public long CityId { get; set; }
    public CityEntity City { get; set; } = null!;

    [ForeignKey("PostDepartment")]
    public long PostDepartmentId { get; set; }
    public PostDepartmentEntity PostDepartment { get; set; } = null!;

    [ForeignKey("PaymentType")]
    public long PaymentTypeId { get; set; }
    public PaymentTypeEntity PaymentType { get; set; } = null!;

    public string PhonNumber { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
}
