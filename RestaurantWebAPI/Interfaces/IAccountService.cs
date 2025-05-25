using RestaurantWebAPI.Models.Account;

namespace RestaurantWebAPI.Interfaces
{
    public interface IAccountService
    {
        public Task<string> LoginAsync(LoginModel model);
    }
}
