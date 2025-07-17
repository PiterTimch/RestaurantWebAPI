namespace Core.Models.Delivery;

public class DeliveryInfoModel
{
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public PostDepartmentModel PostDepartment { get; set; } = null!;
    public PaynamentTypeModel PaymentType { get; set; } = null!;
    public CityModel City { get; set; } = null!;

}
