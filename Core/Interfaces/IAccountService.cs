using Core.Models.Account;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        public Task<List<UserItemModel>> GetAllUsersAsync(); //тимчасово
        public Task<string> LoginAsync(LoginModel model);
        public Task<string> RegisterAsync(RegisterModel model);
        public Task DeleteUserAsync(DeleteUserModel model); //тимчасово
        public Task<string> LoginByGoogle(string token);
        public Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
        public Task<bool> ValidateResetTokenAsync(ValidateResetTokenModel model);
        public Task ResetPasswordAsync(ResetPasswordModel model);
    }
}
