using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.Identity;

namespace Domain;

public class AppDbRestaurantContext : IdentityDbContext<UserEntity, RoleEntity, long>
{
    public AppDbRestaurantContext(DbContextOptions<AppDbRestaurantContext> options) : base(options)
    {
    }

    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<IngredientEntity> Ingredients { get; set; }
    public DbSet<ProductSizeEntity> ProductSizes { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

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
