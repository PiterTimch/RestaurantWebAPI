using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities.Identity;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Account;

namespace RestaurantWebAPI.Services.CRUD
{
    public class AccountService(IJWTTokenService tokenService,
        UserManager<UserEntity> userManager, 
        IMapper mapper,
        IImageService imageService,
        AppDbRestaurantContext context) : IAccountService
    {
        public async Task DeleteUserAsync(DeleteUserModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());

            await userManager.DeleteAsync(user);
        }

        public async Task<List<UserItemModel>> GetAllUsersAsync()
        {
            var entities = await context.Users.ToListAsync();
            var model = mapper.Map<List<UserItemModel>>(entities);

            return model;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await tokenService.CreateTokenAsync(user);
                return token;
            }
            return string.Empty;
        }

        public async Task<string> RegisterAsync(RegisterModel model)
        {
            var user = mapper.Map<UserEntity>(model);
            if (model.ImageFile != null) 
            {
                user.Image = await imageService.SaveImageAsync(model.ImageFile);
            }
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                var token = await tokenService.CreateTokenAsync(user);
                return token;
            }
            return string.Empty;
        }
    }
}
