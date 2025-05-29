//це тимчасовий клас для ліста юзерів, далі перенису для адмін-панелі

namespace Core.Models.Account;

public class UserItemModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email {  get; set; } = string.Empty;
}
