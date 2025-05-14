using System.ComponentModel.DataAnnotations;

namespace RestaurantWebAPI.Models.Category;

public class CategoryCreateModel
{
    [Required]
    [StringLength(250, ErrorMessage = "Name cannot be longer than 250 characters.")]
    public string Name { get; set; } = String.Empty;

    [Required]
    [StringLength(250, ErrorMessage = "Slug cannot be longer than 250 characters.")]
    public string Slug { get; set; } = String.Empty;
    public IFormFile? ImageFile { get; set; } = null;
}
