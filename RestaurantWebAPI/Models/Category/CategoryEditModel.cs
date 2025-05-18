using System.ComponentModel.DataAnnotations;

namespace RestaurantWebAPI.Models.Category
{
    public class CategoryEditModel
    {
        [Required]
        public long Id { get; set; }

        [StringLength(250, ErrorMessage = "Name cannot be longer than 250 characters.")]
        [Required]
        public string Name { get; set; } = String.Empty;

        [StringLength(250, ErrorMessage = "Slug cannot be longer than 250 characters.")]
        [Required]
        public string Slug { get; set; } = String.Empty; 
        
        public IFormFile? ImageFile { get; set; } = null;
    }
}
