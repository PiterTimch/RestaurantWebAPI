using RestaurantWebAPI.Data.Entities.Identity;

namespace RestaurantWebAPI.Interfaces
{
    public interface IJWTTokenService
    {
        Task<string> CreateTokenAsync(UserEntity user);
    }
}
