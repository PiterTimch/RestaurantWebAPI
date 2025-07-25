using Core.Models.AdminUser;
using Core.Models.Search;
using Core.Models.Search.Params;
using Core.Models.Seeder;

namespace Core.Interfaces;

public interface IUserService
{
    Task<List<AdminUserItemModel>> GetAllUsersAsync();
    Task<SearchResult<AdminUserItemModel>> SearchUsersAsync(UserSearchModel model);
    Task<AdminUserItemModel> GetUserById(int id);
    Task DeleteUser(long id);
    Task<string> SeedAsync(SeedItemsModel model);
    Task<string> EditUserAsync(AdminUserEditModel model);
}
