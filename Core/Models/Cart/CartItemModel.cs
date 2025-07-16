namespace Core.Models.Cart;

public class CartItemModel
{
    public long ProductId { get; set; }
    public long CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
}
