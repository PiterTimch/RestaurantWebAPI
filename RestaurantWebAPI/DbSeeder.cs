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

            // ---------- Салати ----------
            var salad = new ProductEntity
            {
                Name = "Грецький салат",
                Slug = "greek-salad",
                CategoryId = 2,
                Price = 145,
                Weight = 350,
                ProductImages = new List<ProductImageEntity>(),
                ProductIngredients = new List<ProductIngredientEntity>()
            };

            var saladImages = new[]
            {
                "https://images.unian.net/photos/2019_12/1577273929-5877.jpg?0.9354250072185213",
                "https://smachno.ua/wp-content/uploads/2009/10/03/Depositphotos_7299284_m-2015.jpg",
                "https://i.obozrevatel.com/food/recipemain/2018/11/16/screenshot1.png"
            };

            foreach (var url in saladImages)
            {
                try
                {
                    var name = await imageService.SaveImageFromUrlAsync(url);
                    salad.ProductImages.Add(new ProductImageEntity { Name = name });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to download image: {url}\n{ex.Message}");
                }
            }

            foreach (var ingredient in ingredients)
            {
                salad.ProductIngredients.Add(new ProductIngredientEntity
                {
                    IngredientId = ingredient.Id
                });
            }

            context.Products.Add(salad);

            // ---------- Суші ----------
            var sushi = new ProductEntity
            {
                Name = "Філадельфія з лососем",
                Slug = "philadelphia-salmon",
                CategoryId = 3,
                Price = 195,
                Weight = 250,
                ProductImages = new List<ProductImageEntity>(),
                ProductIngredients = new List<ProductIngredientEntity>()
            };

            var sushiImages = new[]
            {
                "https://cdn.egersund.ua/a898a417-8e3f-4f08-49ef-a3f38a62a100/1200x900/1200x900",
                "https://smaki-maki.com/wp-content/uploads/sites/4/2024/10/6.jpg",
                "https://assets.dots.live/misteram-public/018ef169-39b3-70e8-8511-d084dc42f415-826x0.png"
            };

            foreach (var url in sushiImages)
            {
                try
                {
                    var name = await imageService.SaveImageFromUrlAsync(url);
                    sushi.ProductImages.Add(new ProductImageEntity { Name = name });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to download image: {url}\n{ex.Message}");
                }
            }

            foreach (var ingredient in ingredients)
            {
                sushi.ProductIngredients.Add(new ProductIngredientEntity
                {
                    IngredientId = ingredient.Id
                });
            }

            context.Products.Add(sushi);

            // ---------- Паназія ----------
            var panasia = new ProductEntity
            {
                Name = "Локшина з овочами",
                Slug = "noodles-veggie",
                CategoryId = 4,
                Price = 165,
                Weight = 400,
                ProductImages = new List<ProductImageEntity>(),
                ProductIngredients = new List<ProductIngredientEntity>()
            };

            var panasiaImages = new[]
            {
                "https://cdn.smak.menu/images/2888/2888-e750fdb74c770966251150d194c89a17.jpg",
                "https://katana.ua/wp-content/uploads/2021/09/DSCF4619-min.jpg",
                "https://assets.dots.live/misteram-public/72a4c5772022f18fe40d5cb5a3a3a9c9.png"
            };

            foreach (var url in panasiaImages)
            {
                try
                {
                    var name = await imageService.SaveImageFromUrlAsync(url);
                    panasia.ProductImages.Add(new ProductImageEntity { Name = name });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to download image: {url}\n{ex.Message}");
                }
            }

            foreach (var ingredient in ingredients)
            {
                panasia.ProductIngredients.Add(new ProductIngredientEntity
                {
                    IngredientId = ingredient.Id
                });
            }

            context.Products.Add(panasia);

            await context.SaveChangesAsync();
        }

        if (!context.OrderStatuses.Any())
        {
            List<string> names = new List<string>() { 
                "Нове", "Очікує оплати", "Оплачено", 
                "В обробці", "Готується до відправки", 
                "Відправлено", "У дорозі", "Доставлено", 
                "Завершено", "Скасовано (вручну)", "Скасовано (автоматично)", 
                "Повернення", "В обробці повернення" };

            var orderStatuses = names.Select(name => new OrderStatusEntity { Name = name }).ToList();

            await context.OrderStatuses.AddRangeAsync(orderStatuses);
            await context.SaveChangesAsync();
        }

        if (!context.Orders.Any()) 
        {
            List<OrderEntity> orders = new List<OrderEntity> 
            {
                new OrderEntity
                {
                    UserId = 1,
                    OrderStatusId = 1,
                },
                new OrderEntity
                {
                    UserId = 1,
                    OrderStatusId = 10,
                },
                new OrderEntity
                {
                    UserId = 1,
                    OrderStatusId = 9,
                },
            };

            context.Orders.AddRange(orders);
            await context.SaveChangesAsync();
        }

        if (!context.OrderItems.Any())
        {
            var products = await context.Products.ToListAsync();
            var orders = await context.Orders.ToListAsync();
            var rand = new Random();

            foreach (var order in orders)
            {
                var existing = await context.OrderItems
                    .Where(x => x.OrderId == order.Id)
                    .ToListAsync();

                if (existing.Count > 0) continue;

                var productCount = rand.Next(1, Math.Min(5, products.Count + 1));

                var selectedProducts = products
                    .Where(p => p.Id != 1) 
                    .OrderBy(_ => rand.Next())
                    .Take(productCount)
                    .ToList();


                var orderItems = selectedProducts.Select(product => new OrderItemEntity
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    PriceBuy = product.Price,
                    Count = rand.Next(1, 5),
                }).ToList();

                context.OrderItems.AddRange(orderItems);
            }

            await context.SaveChangesAsync();
        }

    }
}
