using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Account;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Core.Services.CRUD
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

        public async Task<string> LoginByGoogle(string token)
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            var googleUser = JsonSerializer.Deserialize<GoogleAccountModel>(json);

            var existingUser = await userManager.FindByEmailAsync(googleUser!.Email);
            if (existingUser != null)
            {
                var jwtToken = await tokenService.CreateTokenAsync(existingUser);
                return jwtToken;
            }
            else 
            {
                var user = mapper.Map<UserEntity>(googleUser);

                if (!String.IsNullOrEmpty(googleUser.Picture))
                {
                    user.Image = await imageService.SaveImageFromUrlAsync(googleUser.Picture);
                }

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    var jwtToken = await tokenService.CreateTokenAsync(user);
                    return jwtToken;
                }
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
