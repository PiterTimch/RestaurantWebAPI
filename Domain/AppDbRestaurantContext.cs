using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.Cart;

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
    public DbSet<ProductIngredientEntity> ProductIngredients { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }

    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }

    public DbSet<OrderStatusEntity> OrderStatuses { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }

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

        builder.Entity<ProductIngredientEntity>()
            .HasKey(pi => new { pi.ProductId, pi.IngredientId });

        builder.Entity<UserEntity>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<CartEntity>(c => c.UserId);

        builder.Entity<CartItemEntity>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId);


    }
}
