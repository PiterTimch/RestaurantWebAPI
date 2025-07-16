using Domain.Entities.Cart;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity;

public class UserEntity : IdentityUser<long>
{
    public DateTime DateCreated { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;

    public ICollection<CartEntity>? Carts { get; set; }
    public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
    public ICollection<OrderEntity>? Orders { get; set; }
    public ICollection<UserLoginEntity>? UserLogins { get; set; }
}
