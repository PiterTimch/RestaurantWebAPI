namespace Core.Models.Account;

public class ValidateResetTokenModel
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
