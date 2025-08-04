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

            //Піци

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
                new { Name = "Туно", Slug = "tuno", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_299.webp?ef575e8837d065a1683c022d2077d342",
                    "https://assets.dots.live/misteram-public/79eb2f07-2d17-41ac-ac13-00776ac0ad48-826x0.png" },
                    ingNames = new[] { "Спеції", "Цибуля марс", "Оливки", "Сир Моцарела", "Соус" }
                },
                new { Name = "Компаньйола", Slug = "kompanyola", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_32.webp?0c74b7f78409a4022a2c4c5a5ca3ee19",
                    "https://kvadratsushi.com/mukachevo/wp-content/uploads/sites/18/2023/05/piza_kompaniola-450x300.jpg" },
                    ingNames = new[] { "Гриби печериці", "Болгарський перець", "Кукурудза", "Курка", "Салямі", "Сир Моцарела", "Вершки" }
                },
                new { Name = "Капричоза", Slug = "kaprichoza", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_27.webp?043c3d7e489c69b48737cc0c92d0f3a2",
                    "https://kaifui.com/image/cache/catalog/picanovaja/picakaprichozakf-500x500.jpeg" },
                    ingNames = new[] { "Спеції", "Маслини", "Шинка", "Курка", "Сир Моцарела", "Соус" }
                }
                ,
                new { Name = "Вегетаріано", Slug = "vegetariano", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_22.webp?430c3626b879b4005d41b8a46172e0c0",
                    "https://pizzabro.com.ua/image/cache/pizza/novspizi/pizza020-1000x1000.png" },
                    ingNames = new[] { "Спеції", "Кукурудза", "Артишок", "Курка", "Рукола", "Болгарський перець", "Сир Пармезан", "Сир Моцарела" }
                }
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

            //Салати

            var salads = new[]
            {
                new { Name = "Цезар", Slug = "cezar", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_172.webp?5a4b25aaed25c2ee1b74de72dc03c14e",
                    "https://chizpizza.kh.ua/wp-content/uploads/2023/04/salat-czezar-z-kurkoyu.jpg" },
                    ingNames = new[] { "Соус Цезар", "Крутони", "Сир Пармезан", "Яйце", "Рукола", "Айсберг", "Курка" }
                },
                new { Name = "Грецький", Slug = "greckiy", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_167.webp?f7664060cc52bc6f3d620bcedc94a4b6",
                    "https://i.ytimg.com/vi/zJkGiJIYjlM/sddefault.jpg" },
                    ingNames = new[] { "Сир Горгондзола", "Маслини", "Огірок", "Болгарський перець", "Помідор" }
                },
                new { Name = "Італьяно", Slug = "italyano", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_168.webp?a8baa56554f96369ab93e4f3bb068c22",
                    "https://ecoliya.com.ua/wp-content/uploads/photo_2025-02-11_18-16-57.jpg" },
                    ingNames = new[] { "Соус", "Сир Моцарела", "Маслини", "Айсберг", "Помідор", "Телятина" }
                },
                new { Name = "Твеллі", Slug = "tvelli", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_170.webp?ae0eb3eed39d2bcef4622b2499a05fe6",
                    "https://shuba.life/static/content/thumbs/1824x912/e/77/bnfyvo---c2x1x50px50p-up--021e8a16db954719e9043cc90f3fa77e.jpg" },
                    ingNames = new[] { "Рукола", "Соус", "Сир вершковий", "Помідор", "Баклажан" }
                },
                new { Name = "Кунсей Сарада", Slug = "kunsey-sarada", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_169.webp?e6b4b2a746ed40e1af829d1fa82daa10",
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR2-E9h9dEBDlSDeI9v2f6GqfgLlLr089E1ig&s" },
                    ingNames = new[] { "Кунжут", "Листя салату", "Болгарський перець", "Помідор", "Огірок", "Лосось" }
                }
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
                    ProductIngredients = salad.ingNames
                        .Select(name => ingredients.FirstOrDefault(ing => ing.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        .Where(ing => ing != null)
                        .Select(ing => new ProductIngredientEntity { IngredientId = ing.Id })
                        .ToList(),
                    ProductImages = new List<ProductImageEntity>()
                };

                try
                {
                    foreach (var imgItem in salad.Images)
                    {
                        var img = await imageService.SaveImageFromUrlAsync(imgItem);
                        s.ProductImages.Add(new ProductImageEntity { Name = img });
                    }
                }
                catch { }

                context.Products.Add(s);
            }

            //Суші

            var sushiList = new[]
            {
                new { Name = "Томаго нігірі", Slug = "tomago-nigiri", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_113.webp?f0935e4cd5920aa6c7c996a5ee53a70f",
                    "https://img2.zakaz.ua/upload410fb5986f18d0a03bef65d9347e0c7e.jpg.350x.jpg" },
                    ingNames = new[] { "Унагі соус", "Норі", "Рис", "Японський омлет" }
                },
                new { Name = "Сяке нігірі", Slug = "syake-nigiri", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_102.webp?e6b4b2a746ed40e1af829d1fa82daa10",
                    "https://assets.dots.live/misteram-public/91879d29-fc02-46ce-b710-e1755059657a-826x0.png" },
                    ingNames = new[] { "Рис", "Лосось" }
                },
                new { Name = "Ебі нігірі", Slug = "ebi-nigiri", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_12.webp?c8ed21db4f678f3b13b9d5ee16489088",
                    "https://gotorori.com.ua/image/cache/catalog/NIGIRI/ebi-1600x1066.jpg" },
                    ingNames = new[] { "Рис", "Тигрова креветка" }
                },
                new { Name = "Сяке спайсі", Slug = "syake-spaysi", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_103.webp?288cc0ff022877bd3df94bc9360b9c5d",
                    "https://dragon.pl.ua/image/cache/catalog/product/%D1%81%D0%BF%D0%B0%D0%B9%D1%81%D0%B8%20%D0%B3%D1%83%D0%BD%D0%BA%D0%B0%D0%BD%20%D1%81%D1%8F%D0%BA%D0%B5-650x650.png" },
                    ingNames = new[] { "Норі", "Рис", "Спайсі соус", "Лосось" }
                },
                new { Name = "Макі з філадельфії", Slug = "maki-z-filadelfiyi", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_71.webp?33e75ff09dd601bbe69f351039152189",
                    "https://crazycrab.com.ua/image/cache/catalog/maki/%D0%9C%D0%B0%D0%BA%D1%96%20%D1%84%D1%96%D0%BB%D0%B0%D0%B4%D0%B5%D0%BB%D1%8C%D1%84%D1%96%D1%8F2024-700x700.JPG" },
                    ingNames = new[] { "Норі", "Рис", "Болгарський перець", "Сир вершковий" }
                },
                new { Name = "Унагі філадельфія", Slug = "unagi-filadelfiya", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_123.webp?fe7ee8fc1959cc7214fa21c4840dff0a",
                    "https://lh3.googleusercontent.com/proxy/fzWJ8L3dXq2--5zNkuBFdmFi9Hymqo2G1iSYJBVUsTJkQiwc_2FPukWYMb62lk0_9EJwZQ0crvjoiuSIhrgEbiDn_MpomQXqOvdAATSp7jxUR1vN39KF" },
                    ingNames = new[] { "Унагі соус", "Норі", "Рис", "Сир вершковий", "Вугор" }
                },
                new { Name = "Каппа макі", Slug = "kappa-maki", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_65.webp?fe131d7f5a6b38b23cc967316c13dae2",
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTOGA882g_Q63HRFLWCxLctcVI90_-a68LZw&s" },
                    ingNames = new[] { "Норі", "Рис", "Кунжут", "Огірок" }
                },
                new { Name = "Сяке каппа макі", Slug = "syake-kappa-maki", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_94.webp?dd45045f8c68db9f54e70c67048d32e8",
                    "https://lh3.googleusercontent.com/proxy/m6e-jrtBskfros3T9K2UDj65paIC2vezGxWXrFZ8__lhIAqB6hj3XwS-sfHick2f0nmu8qrYp79Hvmq6JX9ofbSaGWCWN_pEkN3twDb_zqZXJO0iPMmTDOCEGrPkb0fZYVTXpi0jO7iheGSXyLoAFZAVjffJ6tw" },
                    ingNames = new[] { "Норі", "Рис", "Лосось", "Огірок" }
                },
                new { Name = "Каліфорнія сяке сурімі макі", Slug = "kaliforniya-syake-surimi-maki", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_59.webp?9dfcd5e558dfa04aaf37f137a1d9d3e5",
                    "https://osminog.online/wp-content/uploads/2022/08/kani-sake-2024-scaled.jpg" },
                    ingNames = new[] { "Норі", "Рис", "Лосось", "Огірок", "Кунжут", "Сир вершковий", "Сурімі" }
                },
                new { Name = "Сяке тобіко макі", Slug = "syake-tobiko-maki", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_106.webp?c32d9bf27a3da7ec8163957080c8628e",
                    "https://tonypepperoni.com.ua/wa-data/public/shop/products/00/webp/33/00/33/images/509/509.370x0@2x.webp" },
                    ingNames = new[] { "Норі", "Рис", "Лосось", "Огірок", "Авокадо", "Сир вершковий", "Ікра тобіко" }
                }
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
                    ProductIngredients = sushiItem.ingNames
                        .Select(name => ingredients.FirstOrDefault(ing => ing.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        .Where(ing => ing != null)
                        .Select(ing => new ProductIngredientEntity { IngredientId = ing.Id })
                        .ToList(),
                    ProductImages = new List<ProductImageEntity>()
                };

                try
                {
                    foreach (var imgItem in sushiItem.Images)
                    {
                        var img = await imageService.SaveImageFromUrlAsync(imgItem);
                        sushi.ProductImages.Add(new ProductImageEntity { Name = img });
                    }
                }
                catch { }

                context.Products.Add(sushi);
            }

            //Паназія

            var panasiaList = new[]
            {
                new { Name = "Удон з куркою", Slug = "udon-z-kurkoyu", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_182.webp?757b505cfd34c64c85ca5b5690ee5293",
                    "https://novosti24.kyiv.ua/wp-content/uploads/2023/07/recipe_f0caba4f-243a-4fe8-bed5-e04452bff4a0_w450.jpg" },
                    ingNames = new[] { "Стружка тунця", "Соус Тонкацу", "Часник", "Морква", "Цибуля", "Гриби печериці", "Болгарський перець", "Курка", "Локшина удон" }
                },
                new { Name = "Удон зі свининою", Slug = "udon-zi-svininoyu", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_182.webp?757b505cfd34c64c85ca5b5690ee5293",
                    "https://novosti24.kyiv.ua/wp-content/uploads/2023/07/recipe_f0caba4f-243a-4fe8-bed5-e04452bff4a0_w450.jpg" },
                    ingNames = new[] { "Стружка тунця", "Соус Тонкацу", "Часник", "Морква", "Цибуля", "Гриби печериці", "Болгарський перець", "Свинина", "Локшина удон" }
                },
                new { Name = "Карбонара", Slug = "karbonara", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_176.webp?248e844336797ec98478f85e7626de4a",
                    "https://myastoriya.com.ua/upload/iblock/153/m7tqlwgeqe40p1v6v1zjpb59ztce5843.jpg" },
                    ingNames = new[] { "Сир Пармезан", "Яйце", "Вершки", "Бекон", "Спагетті" }
                },
                new { Name = "Маре", Slug = "mare", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_178.webp?31839b036f63806cba3f47b93af8ccb5",
                    "https://smachnonews.24tv.ua/resources/photos/news/202306/2341962.jpg?v=1687783159000" },
                    ingNames = new[] { "Сир Пармезан", "Вершки", "Мідії", "Лосось", "Тигрова креветка", "Спагетті" }
                },
                new { Name = "Рамен", Slug = "sup-ramen", Images = new[] {
                    "https://matsuri.com.ua/img_files/gallery_commerce/products/big/commerce_products_images_71.webp?33e75ff09dd601bbe69f351039152189",
                    "https://crazycrab.com.ua/image/cache/catalog/maki/%D0%9C%D0%B0%D0%BA%D1%96%20%D1%84%D1%96%D0%BB%D0%B0%D0%B4%D0%B5%D0%BB%D1%8C%D1%84%D1%96%D1%8F2024-700x700.JPG" },
                    ingNames = new[] { "Норі", "Свинина", "Яйце", "Локшина удон" }
                }
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
                    ProductIngredients = item.ingNames
                        .Select(name => ingredients.FirstOrDefault(ing => ing.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        .Where(ing => ing != null)
                        .Select(ing => new ProductIngredientEntity { IngredientId = ing.Id })
                        .ToList(),
                    ProductImages = new List<ProductImageEntity>()
                };

                try
                {
                    foreach (var imgItem in item.Images)
                    {
                        var img = await imageService.SaveImageFromUrlAsync(imgItem);
                        pan.ProductImages.Add(new ProductImageEntity { Name = img });
                    }
                }
                catch { }

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

        if (!context.PostDepartments.Any())
        {
            await novaPosta.FetchDepartmentsAsync();
        }

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
