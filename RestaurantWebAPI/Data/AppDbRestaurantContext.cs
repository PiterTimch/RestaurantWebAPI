using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data.Entities;
using RestaurantWebAPI.Data.Entities.Identity;

namespace RestaurantWebAPI.Data;

public class AppDbRestaurantContext : IdentityDbContext<UserEntity, RoleEntity, long>
{
    public AppDbRestaurantContext(DbContextOptions<AppDbRestaurantContext> options) : base(options)
    {
    }

    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserRoleEntity>(ur =>
        {
            ur.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();

            ur.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        });
    }
}
