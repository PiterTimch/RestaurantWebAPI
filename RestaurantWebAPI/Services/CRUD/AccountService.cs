using Microsoft.AspNetCore.Identity;
using RestaurantWebAPI.Data.Entities.Identity;
using RestaurantWebAPI.Interfaces;
using RestaurantWebAPI.Models.Account;

namespace RestaurantWebAPI.Services.CRUD
{
    public class AccountService(IJWTTokenService tokenService,
        UserManager<UserEntity> userManager) : IAccountService
    {
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
    }
}
