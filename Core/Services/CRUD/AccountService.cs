using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities.Identity;
using Core.Interfaces;
using Core.Models.Account;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Core.Models.Smtp;

namespace Core.Services.CRUD
{
    public class AccountService(IJWTTokenService tokenService,
        UserManager<UserEntity> userManager,
        IMapper mapper,
        IConfiguration configuration,
        IImageService imageService,
        ISmtpService smtpService,
        IAuthService authService,
        AppDbRestaurantContext context) : IAccountService
    {
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return false;
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{configuration["ClientUrl"]}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(model.Email)}";

            var emailModel = new EmailMessage
            {
                To = model.Email,
                Subject = "Password Reset",
                Body = $"<p>Click the link below to reset your password:</p><a href='{resetLink}'>Reset Password</a>"
            };

            var result = await smtpService.SendEmailAsync(emailModel);

            return result;
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

            var response = await httpClient.GetAsync(configuration["GoogleUserInfo"] ?? "https://www.googleapis.com/oauth2/v2/userinfo");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            var googleUser = JsonSerializer.Deserialize<GoogleAccountModel>(json);

            var existingUser = await userManager.FindByEmailAsync(googleUser!.Email);
            if (existingUser != null)
            {
                var userLoginGoogle = await userManager.FindByLoginAsync("Google", googleUser.GoogleId);

                if (userLoginGoogle == null)
                {
                    await userManager.AddLoginAsync(existingUser, new UserLoginInfo("Google", googleUser.GoogleId, "Google"));
                }

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

                    result = await userManager.AddLoginAsync(user, new UserLoginInfo("Google", googleUser.GoogleId, "Google"));

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

        public async Task<bool> ValidateResetTokenAsync(ValidateResetTokenModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            return await userManager.VerifyUserTokenAsync(
                user,
                TokenOptions.DefaultProvider,
                "ResetPassword",
                model.Token);
        }

        public async Task ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email!);

            if (user != null)
                await userManager.ResetPasswordAsync(user, model.Token!, model.NewPassword);
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model)
        {
            var user = await userManager.FindByIdAsync((await authService.GetUserId()).ToString());


            if (user != null)
            {
                var res = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (!res.Succeeded)
                {
                    throw new Exception("Failed to change password: " + string.Join(", ", res.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
