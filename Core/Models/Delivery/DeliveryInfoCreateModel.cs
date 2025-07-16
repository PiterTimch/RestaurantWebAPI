namespace Core.Models.Delivery;

public class DeliveryInfoCreateModel
{
    /// <summary>
    /// CityId
    /// </summary>
    /// <example>1</example>
    public long CityId { get; set; }

    /// <summary>
    /// PostDepartmentId
    /// </summary>
    /// <example>1</example>
    public long PostDepartmentId { get; set; }

    /// <summary>
    /// PaymentTypeId
    /// </summary>
    /// <example>1</example>
    public long PaymentTypeId { get; set; }

    /// <summary>
    /// PaymentTypeId
    /// </summary>
    /// <example>+380973740391</example>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// PaymentTypeId
    /// </summary>
    /// <example>Tralalelo tralala</example>
    public string RecipientName { get; set; } = string.Empty;
}
