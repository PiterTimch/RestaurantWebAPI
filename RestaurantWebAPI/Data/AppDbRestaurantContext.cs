using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data.Entities;

namespace RestaurantWebAPI.Data;

public class AppDbRestaurantContext : DbContext
{
    public AppDbRestaurantContext(DbContextOptions<AppDbRestaurantContext> options) : base(options)
    {
    }

    public DbSet<CategoryEntity> Categories { get; set; }
}
