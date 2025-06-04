using Microsoft.AspNetCore.Http;

namespace Core.Models.ProductImage;

public class ProductImageCreateModel
{
    public IFormFile ImageFile { get; set; } = null!;
    public int Priority { get; set; } = 0;
}
