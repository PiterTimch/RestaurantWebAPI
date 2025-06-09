using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Constants;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Seeder;
using System.Text.Json;
using Core.Models.ProductImage;

namespace RestaurantWebAPI;

public static class DbSeeder
{
    public static async Task SeedData(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        //Цей об'єкт буде верта посилання на конткетс, який зараєстрвоано в Progran.cs
        var context = scope.ServiceProvider.GetRequiredService<AppDbRestaurantContext>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

        context.Database.Migrate();

        if (!context.Categories.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var categories = JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);
                    var entityItems = mapper.Map<List<CategoryEntity>>(categories);
                    foreach (var entity in entityItems)
                    {
                        entity.Image =
                            await imageService.SaveImageFromUrlAsync(entity.Image);
                    }

                    await context.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Categories.json");
            }
        }

        if (!context.Roles.Any())
        {
            var roles = Roles.AllRoles.Select(r => new RoleEntity(r)).ToList();

            foreach (var role in roles)
            {
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Error Create Role {0}", role.Name);
                }
            }
        }

        if (!context.Users.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    
                    foreach(var user in users)
                    {
                        var entity = mapper.Map<UserEntity>(user);
                        entity.Image = await imageService.SaveImageFromUrlAsync(user.Image);
                        var result = await userManager.CreateAsync(entity, user.Password);
                        if (!result.Succeeded)
                        {
                            Console.WriteLine("Error Create User {0}", user.Email);
                            continue;
                        }
                        foreach (var role in user.Roles)
                        {
                            if (await roleManager.RoleExistsAsync(role))
                            {
                                await userManager.AddToRoleAsync(entity, role);
                            }
                            else
                            {
                                Console.WriteLine("Role {0} not found", role);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Categories.json");
            }
        }

        if (!context.Ingredients.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Ingredients.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var categories = JsonSerializer.Deserialize<List<SeederIngredientModel>>(jsonData);
                    var entityItems = mapper.Map<List<IngredientEntity>>(categories);
                    foreach (var entity in entityItems)
                    {
                        entity.Image =
                            await imageService.SaveImageFromUrlAsync(entity.Image);
                    }

                    await context.Ingredients.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Ingredients.json");
            }
        }

        if (!context.ProductSizes.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "ProductSizes.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var categories = JsonSerializer.Deserialize<List<SeederProductSizeModel>>(jsonData);
                    var entityItems = mapper.Map<List<ProductSizeEntity>>(categories);

                    await context.ProductSizes.AddRangeAsync(entityItems);
                    await context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File ProductSizes.json");
            }
        }

        if (!context.Products.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

            // Створюємо головний продукт (без розміру)
            var caesarParent = new ProductEntity
            {
                Name = "Цезаре",
                Slug = "caesar",
                CategoryId = 1,
                ProductIngredients = new List<ProductIngredientEntity>()
            };

            context.Products.Add(caesarParent);
            await context.SaveChangesAsync();

            var sizes = await context.ProductSizes.ToListAsync();

            var images = new[]
            {
                "https://prontopizza.ua/ternopil/wp-content/uploads/sites/15/2023/10/czezar-kopiya-500x500.webp",
                "https://kvadratsushi.com/wp-content/uploads/2020/06/chezar_1200x800.jpg",
                "https://assets.dots.live/misteram-public/018bee4e-8d79-7202-985f-66327f044f25-826x0.png"
            };

            var ingredients = context.Ingredients.ToList();

            foreach (var size in sizes)
            {
                var child = new ProductEntity
                {
                    Name = $"Цезаре ({size.Name})",
                    Slug = $"caesar-{size.Name.ToLower().Replace(" ", "").Replace("см", "cm")}",
                    Price = 195 + size.Id * 20,
                    Weight = 500 + Convert.ToInt32(size.Id) * 50,
                    CategoryId = caesarParent.CategoryId,
                    ProductSizeId = size.Id,
                    ParentProductId = caesarParent.Id,
                    ProductImages = new List<ProductImageEntity>(),
                    ProductIngredients = new List<ProductIngredientEntity>()
                };

                foreach (var ingredient in ingredients)
                {
                    child.ProductIngredients.Add(new ProductIngredientEntity
                    {
                        IngredientId = ingredient.Id
                    });
                }

                foreach (var imageUrl in images)
                {
                    var savedName = await imageService.SaveImageFromUrlAsync(imageUrl);
                    child.ProductImages.Add(new ProductImageEntity
                    {
                        Name = savedName
                    });
                }

                context.Products.Add(child);
                await context.SaveChangesAsync();
            }

            foreach (var ingredient in ingredients)
            {
                caesarParent.ProductIngredients.Add(new ProductIngredientEntity
                {
                    IngredientId = ingredient.Id
                });
            }

            await context.SaveChangesAsync();
        }

    }
}
