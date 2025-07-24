using Core.Models.Account;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        public Task<string> LoginAsync(LoginModel model);
        public Task<string> RegisterAsync(RegisterModel model);
        public Task<string> LoginByGoogle(string token);
        public Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
        public Task<bool> ValidateResetTokenAsync(ValidateResetTokenModel model);
        public Task ResetPasswordAsync(ResetPasswordModel model);
        public Task ChangePasswordAsync(ChangePasswordModel model);
    }
}
