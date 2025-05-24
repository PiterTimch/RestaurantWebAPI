using Microsoft.AspNetCore.Identity;

namespace RestaurantWebAPI.Data.Entities.Identity;

public class UserEntity : IdentityUser<long>
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;

    public virtual ICollection<UserRoleEntity>? UserRoles { get; set; } 
}
