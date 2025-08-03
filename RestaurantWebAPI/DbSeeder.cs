using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Constants;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Seeder;
using System.Text.Json;
using Core.Models.ProductImage;
using Domain.Entities.Delivery;

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
        var novaPosta = scope.ServiceProvider.GetRequiredService<INovaPoshtaService>();

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

            var sizes = await context.ProductSizes.ToListAsync();
            var ingredients = context.Ingredients.ToList();

            var random = new Random();

            var pizzas = new[]
            {
                new { Name = "Гофредо", Slug = "gofredo", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_296.webp?5878a7ab84fb43402106c575658472fa",
                    "https://ks.biz.ua/wp-content/uploads/2021/06/gofredo-238x238-1.jpg" },
                    ingNames = new[] { "Цибуля марс", "Помідор", "Салямі", "Шинка", "Сир Моцарела" }
                },
                new { Name = "Пепероні", Slug = "pepperoni", Images = new[] {
                    "https://ecopizza.com.ua/555-large_default/pica-peperoni.jpg",
                    "https://pizza.od.ua/upload/resize_cache/webp/upload/iblock/62e/4y05ehhgm88eupox5eh111jo1k2e94mq.webp" },
                    ingNames = new[] { "Сир Пармезан", "Салямі пепероні", "Сир Моцарела", "Соус" }
                },
                new { Name = "Гавайська", Slug = "hawaiian", Images = new[] {
                    "https://adriano.com.ua/wp-content/uploads/2022/08/%D0%93%D0%B0%D0%B2%D0%B0%D0%B8%CC%86%D1%81%D1%8C%D0%BA%D0%B0.jpeg",
                    "https://www.moi-sushi.com.ua/wp-content/uploads/2022/08/gavajska.jpg" },
                    ingNames = new[] { "Кукурудза", "Спеції", "Курка", "Сир Моцарела", "Вершки" }
                },
                new { Name = "4 Сири", Slug = "4cheese", Images = new[] {
                    "https://ecopizza.com.ua/559-cart_default/picca-4-syra.jpg",
                    "https://lutsk.samudoma.com.ua/wp-content/uploads/2024/01/4-syry-1.jpg" },
                    ingNames = new[] { "Сир вершковий", "Сир Камамбер", "Сир Пармезан", "Сир Моцарела" }
                },
            };

            foreach (var pizza in pizzas)
            {
                var parent = new ProductEntity
                {
                    Name = pizza.Name,
                    Slug = pizza.Slug,
                    CategoryId = 1,
                    ProductIngredients = pizza.ingNames
                        .Select(name => ingredients.FirstOrDefault(ing => ing.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        .Where(ing => ing != null)
                        .Select(ing => new ProductIngredientEntity { IngredientId = ing.Id })
                        .ToList()
                };
                context.Products.Add(parent);
                await context.SaveChangesAsync();

                int price = random.Next(150, 250);

                foreach (var size in sizes)
                {
                    var child = new ProductEntity
                    {
                        Name = $"{pizza.Name} ({size.Name})",
                        Slug = $"{pizza.Slug}-{size.Name.ToLower().Replace(" ", "").Replace("см", "cm")}",
                        Price = price + size.Id * 20,
                        Weight = 400 + Convert.ToInt32(size.Id) * 60,
                        CategoryId = 1,
                        ProductSizeId = size.Id,
                        ParentProductId = parent.Id,
                        ProductIngredients = pizza.ingNames
                        .Select(name => ingredients.FirstOrDefault(ing => ing.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        .Where(ing => ing != null)
                        .Select(ing => new ProductIngredientEntity { IngredientId = ing.Id })
                        .ToList(),
                        ProductImages = new List<ProductImageEntity>()
                    };

                    foreach (var imageUrl in pizza.Images)
                    {
                        var saved = await imageService.SaveImageFromUrlAsync(imageUrl);
                        child.ProductImages.Add(new ProductImageEntity { Name = saved });
                    }

                    context.Products.Add(child);
                }

                await context.SaveChangesAsync();
            }

            var salads = new[]
            {
                new { Name = "Цезар з куркою", Slug = "caesar-chicken", Image = "https://i.obozrevatel.com/food/recipemain/2019/4/15/bed91432.jpg?size=636x424" },
                new { Name = "Олів’є", Slug = "olivier", Image = "https://fayni-recepty.com.ua/wp-content/uploads/2020/08/olivie.jpg" },
                new { Name = "Вінегрет", Slug = "vinegret", Image = "https://zira.uz/wp-content/uploads/2018/10/vinegret.jpg" },
                new { Name = "Салат з тунцем", Slug = "tuna-salad", Image = "https://chizpizza.kh.ua/wp-content/uploads/2023/04/salat-z-tunczem.jpg" }
            };

            foreach (var salad in salads)
            {
                var s = new ProductEntity
                {
                    Name = salad.Name,
                    Slug = salad.Slug,
                    CategoryId = 2,
                    Price = random.Next(120, 240),
                    Weight = 350,
                    ProductIngredients = new List<ProductIngredientEntity>(),
                    ProductImages = new List<ProductImageEntity>()
                };

                try
                {
                    var img = await imageService.SaveImageFromUrlAsync(salad.Image);
                    s.ProductImages.Add(new ProductImageEntity { Name = img });
                }
                catch { }

                foreach (var ing in ingredients)
                    s.ProductIngredients.Add(new ProductIngredientEntity { IngredientId = ing.Id });

                context.Products.Add(s);
            }

            var sushiList = new[]
            {
                new { Name = "Каліфорнія з крабом", Slug = "california-crab", Image = "https://cdn.egersund.ua/124593b3-6c99-45c6-1042-dc77a92d0500/origin/origin" },
                new { Name = "Унагі", Slug = "unagi", Image = "https://storage.smilefood.ua/storage/fd/a7/fda7f741ce1a94b1cc353f16cc5c2801.jpg" },
                new { Name = "Текка Макі", Slug = "tekka-maki", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSHJFwfC3aA8rV-UsTUSMDTmA7PD2k52Wa1Og&s" },
                new { Name = "Дракон рол", Slug = "dragon-roll", Image = "https://i.evrasia.in.ua/data/1400_0/products/54wJtl1FOAh4jcBKNNKsHAgjy2I2deqMpd5VjbfK.webp" }
            };

            foreach (var sushiItem in sushiList)
            {
                var sushi = new ProductEntity
                {
                    Name = sushiItem.Name,
                    Slug = sushiItem.Slug,
                    CategoryId = 3,
                    Price = random.Next(190, 400),
                    Weight = 220,
                    ProductIngredients = new List<ProductIngredientEntity>(),
                    ProductImages = new List<ProductImageEntity>()
                };

                try
                {
                    var img = await imageService.SaveImageFromUrlAsync(sushiItem.Image);
                    sushi.ProductImages.Add(new ProductImageEntity { Name = img });
                }
                catch { }

                foreach (var ing in ingredients)
                    sushi.ProductIngredients.Add(new ProductIngredientEntity { IngredientId = ing.Id });

                context.Products.Add(sushi);
            }

            var panasiaList = new[]
            {
                new { Name = "Локшина з куркою", Slug = "noodles-chicken", Image = "https://katana.ua/wp-content/uploads/2020/08/%D0%9B%D0%B0%D0%BF%D1%88%D0%B0-%D1%81-%D0%BA%D1%83%D1%80%D0%B8%D1%86%D0%B5%D0%B9-%D0%B2-%D1%81%D0%BE%D1%83%D1%81%D0%B5-%D1%82%D0%B5%D1%80%D0%B8%D1%8F%D0%BA%D0%B8-min-1-scaled.jpg" },
                new { Name = "Рис з овочами", Slug = "rice-veggie", Image = "https://fayni-recepty.com.ua/wp-content/uploads/2020/05/rys-ovochi.jpg" },
                new { Name = "Фунчоза з овочами", Slug = "funchoza-veggie", Image = "https://cdn.smak.menu/images/maxone/8939/8939-f9cee6415be21a78a3c4b31551380e81.jpg" },
                new { Name = "Тяхан з морепродуктами", Slug = "tyahan-seafood", Image = "https://omnomnom.dp.ua/image/cache/catalog/wok_new/1-25-500x500.jpg" }
            };

            foreach (var item in panasiaList)
            {
                var pan = new ProductEntity
                {
                    Name = item.Name,
                    Slug = item.Slug,
                    CategoryId = 4,
                    Price = random.Next(160, 300),
                    Weight = 420,
                    ProductIngredients = new List<ProductIngredientEntity>(),
                    ProductImages = new List<ProductImageEntity>()
                };

                try
                {
                    var img = await imageService.SaveImageFromUrlAsync(item.Image);
                    pan.ProductImages.Add(new ProductImageEntity { Name = img });
                }
                catch { }

                foreach (var ing in ingredients)
                    pan.ProductIngredients.Add(new ProductIngredientEntity { IngredientId = ing.Id });

                context.Products.Add(pan);
            }

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

        //if (!context.PostDepartments.Any())
        //{
        //    await novaPosta.FetchDepartmentsAsync();
        //}

        if (!context.PaymentTypes.Any())
        {
            var list = new List<PaymentTypeEntity>
            {
                new PaymentTypeEntity { Name = "Готівка" },
                new PaymentTypeEntity { Name = "Картка" }
            };

            await context.PaymentTypes.AddRangeAsync(list);
            await context.SaveChangesAsync();
        }

    }
}
