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

         await context.Users
        .ForEachAsync(user =>
        {
            var adminUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (adminUser != null)
            {
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    adminUser.LoginTypes.Add("Password");
                }
            }
        });

        return users;
    }
}
