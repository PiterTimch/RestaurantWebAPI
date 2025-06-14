using Domain.Entities.Cart;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity;

public class UserEntity : IdentityUser<long>
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;

    public virtual CartEntity Cart { get; set; } = new CartEntity();

    public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
    public ICollection<OrderEntity>? Orders { get; set; }
}
