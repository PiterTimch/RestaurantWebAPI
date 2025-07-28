using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bogus;
using Core.Interfaces;
using Core.Models.AdminUser;
using Core.Models.Search;
using Core.Models.Search.Params;
using Core.Models.Seeder;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Bogus.DataSets.Name;

namespace Core.Services.CRUD;

public class UserService(UserManager<UserEntity> userManager,
    IMapper mapper,
    IImageService imageService,
    RoleManager<RoleEntity> roleManager,
    AppDbRestaurantContext context,
    IJWTTokenService tokenService) : IUserService
{
    public async Task<List<AdminUserItemModel>> GetAllUsersAsync()
    {
        var users = await userManager.Users
            .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        await LoadLoginsAndRolesAsync(users);

        return users;
    }

    public async Task<SearchResult<AdminUserItemModel>> SearchUsersAsync(UserSearchModel model)
    {
        var query = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            string nameFilter = model.Name.Trim().ToLower().Normalize();

            query = query.Where(u =>
                (u.FirstName + " " + u.LastName).ToLower().Contains(nameFilter) ||
                u.FirstName.ToLower().Contains(nameFilter) ||
                u.LastName.ToLower().Contains(nameFilter));
        }

        if (model?.StartDate != null)
        {
            query = query.Where(u => u.DateCreated >= model.StartDate);
        }

        if (model?.EndDate != null)
        {
            query = query.Where(u => u.DateCreated <= model.EndDate);
        }

        if (model?.Roles != null && model.Roles.Any())
        {
            query = query.Where(u => u.UserRoles.Any(ur => model.Roles.Contains(ur.Role.Name)));
        }

        var totalCount = await query.CountAsync();

        var safeItemsPerPage = model.ItemPerPage < 1 ? 10 : model.ItemPerPage;
        var totalPages = (int)Math.Ceiling(totalCount / (double)safeItemsPerPage);
        var safePage = Math.Min(Math.Max(1, model.Page), Math.Max(1, totalPages));

        var users = await query
            .OrderBy(u => u.Id)
            .Skip((safePage - 1) * safeItemsPerPage)
            .Take(safeItemsPerPage)
            .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        //await LoadLoginsAndRolesAsync(users);

        return new SearchResult<AdminUserItemModel>
        {
            Items = users,
            Pagination = new PaginationModel
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                ItemsPerPage = safeItemsPerPage,
                CurrentPage = safePage
            }
        };
    }

    private async Task LoadLoginsAndRolesAsync(List<AdminUserItemModel> users) 
    {
        await context.UserLogins.ForEachAsync(login =>
        {
            var user = users.FirstOrDefault(u => u.Id == login.UserId);
            if (user != null)
            {
                user.LoginTypes.Add(login.LoginProvider);
            }
        });

        var identityUsers = await userManager.Users.AsNoTracking().ToListAsync();

        foreach (var identityUser in identityUsers) // Забрав foreachAsync через конфлікнт з userManager.GetRolesAsync(identityUser)
        {
            var adminUser = users.FirstOrDefault(u => u.Id == identityUser.Id);
            if (adminUser != null)
            {
                var roles = await userManager.GetRolesAsync(identityUser);
                adminUser.Roles = roles.ToList();

                if (!string.IsNullOrEmpty(identityUser.PasswordHash))
                {
                    adminUser.LoginTypes.Add("Password");
                }
            }
        }
    }

    public async Task<string> SeedAsync(SeedItemsModel model)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var fakeUsers = new Faker<SeederUserModel>("uk")
            .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
           //Pick some fruit from a basket
           .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
           .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
           .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
           .RuleFor(u => u.Password, (f, u) => f.Internet.Password(8))
           .RuleFor(u => u.Roles, f => new List<string>() { f.PickRandom(Constants.Roles.AllRoles) })
           .RuleFor(u => u.Image, f => "https://thispersondoesnotexist.com");

        var genUsers = fakeUsers.Generate(model.Count);

        try
        {
            foreach (var user in genUsers)
            {
                var entity = mapper.Map<UserEntity>(user);
                entity.UserName = user.Email;
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
                        Console.WriteLine("Not Found Role {0}", role);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Json Parse Data {0}", ex.Message);
        }

        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

        return elapsedTime;
    }

    public async Task<string> EditUserAsync(AdminUserEditModel model)
    {
        var existing = await userManager.FindByIdAsync(model.Id.ToString());
        var userLogins = await context.UserLogins
            .FirstOrDefaultAsync(ul => ul.UserId == existing.Id);

        if (userLogins != null && userLogins.LoginProvider == "Google")
            throw new InvalidOperationException("Cannot edit email for Google login user");

        existing = mapper.Map(model, existing);

        if (model.Image != null) 
        {
            imageService.DeleteImageAsync(existing.Image);
            existing.Image = await imageService.SaveImageAsync(model.Image);
        }

        if (model.Roles != null)
        {
            var currentRoles = await userManager.GetRolesAsync(existing);
            await userManager.RemoveFromRolesAsync(existing, currentRoles);
            await userManager.AddToRolesAsync(existing, model.Roles);
        }

        await userManager.UpdateAsync(existing);

        var jwtToken = await tokenService.CreateTokenAsync(existing);
        return jwtToken;
    }

    public async Task<AdminUserItemModel> GetUserById(int id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return null;
        
        var adminUser = mapper.Map<AdminUserItemModel>(user);

        await LoadLoginsAndRolesAsync(new List<AdminUserItemModel> { adminUser });

        return adminUser;
    }

    public async Task DeleteUser(long id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user != null)
        {
            await userManager.DeleteAsync(user);
        }
    }
}
