namespace Core.Models.AdminUser;

public class AdminUserItemModel
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public string Image { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> LoginTypes { get; set; } = new();
    public bool IsGoogleLogin { get; set; } = false;
    public bool IsPasswordLogin { get; set; } = false;
}
