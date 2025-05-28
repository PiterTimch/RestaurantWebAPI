using RestaurantWebAPI.Models.Account;

namespace RestaurantWebAPI.Interfaces
{
    public interface IAccountService
    {
        public Task<string> LoginAsync(LoginModel model);
        public Task<string> RegisterAsync(RegisterModel model);
        public Task DeleteUserAsync(DeleteUserModel model);
    }
}
