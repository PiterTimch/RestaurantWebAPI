namespace Core.Models.Search.Params;

public class ProductSearchModel : BaseSearchParamsModel
{
    public string? Name { get; set; }
    public long? CategoryId { get; set; }
    public long? ProductSizeId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<long>? ProhibitedIngredientIds { get; set; }
}
