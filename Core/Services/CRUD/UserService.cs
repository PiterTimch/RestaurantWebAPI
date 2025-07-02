using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.AdminUser;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.CRUD;

public class UserService(UserManager<UserEntity> userManager, 
    IMapper mapper,
    AppDbRestaurantContext context) : IUserService
{
    public async Task<List<AdminUserItemModel>> GetAllUsersAsync()
    {
        var users = await userManager.Users
            .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

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

        return users;
    }
}
