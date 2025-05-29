using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbRestaurantContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
