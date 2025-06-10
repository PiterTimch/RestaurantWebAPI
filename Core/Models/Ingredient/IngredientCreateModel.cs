using Microsoft.AspNetCore.Http;

namespace Core.Models.Ingredient;

public class IngredientCreateModel
{
    public string Name { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }
}
