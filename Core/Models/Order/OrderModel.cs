using Core.Models.Delivery;

namespace Core.Models.Order;

public class OrderModel
{
    public long Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemModel>? OrderItems { get; set; }
    public DeliveryInfoModel? DeliveryInfo { get; set; } = null!;
}
