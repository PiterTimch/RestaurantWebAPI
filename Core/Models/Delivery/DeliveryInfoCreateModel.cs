namespace Core.Models.Delivery;

public class DeliveryInfoCreateModel
{
    public long CityId { get; set; }
    public long PostDepartmentId { get; set; }
    public long PaymentTypeId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public long OrderId { get; set; }
}
