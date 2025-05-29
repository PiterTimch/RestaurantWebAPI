using Domain.Entities.Identity;

namespace Core.Interfaces
{
    public interface IJWTTokenService
    {
        Task<string> CreateTokenAsync(UserEntity user);
    }
}
