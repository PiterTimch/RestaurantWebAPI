using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Category;

public class CategoryCreateModel
{
    public string Name { get; set; } = String.Empty;
    public string Slug { get; set; } = String.Empty;
    public IFormFile? ImageFile { get; set; } = null;
}
