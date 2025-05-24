using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestaurantWebAPI.Data.Entities.Identity;
using RestaurantWebAPI.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace RestaurantWebAPI.Services;

public class JWTTokenService(IConfiguration configuration,
    UserManager<UserEntity> userManager) : IJWTTokenService
{
    public async Task<string> CreateTokenAsync(UserEntity user)
    {
        string key = configuration["JWT:Key"]!;

        var claims = new List<Claim>
        {
            new Claim("email", user.Email),
            new Claim("name", $"{user.FirstName} {user.LastName}")
        };

        foreach (var role in await userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim("role", role));
        }

        var keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        var signingKey = new SymmetricSecurityKey(keyBytes);

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var sec = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signingCredentials
        );

        string token = new JwtSecurityTokenHandler().WriteToken(sec);

        return token;
    }
}
