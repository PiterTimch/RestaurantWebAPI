using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Models.Product;

public class ProductEditModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Weight { get; set; }
    public long CategoryId { get; set; }
    public long ProductSizeId { get; set; }
    public List<long>? IngredientIds { get; set; }

    [BindProperty(Name = "imageFiles[]")]
    public List<IFormFile>? ImageFiles { get; set; }
}
