namespace Core.Models.Cart;

public class CartListModel
{
    public long Id { get; set; }
    public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();
    public decimal TotalPrice { get; set; }
}
