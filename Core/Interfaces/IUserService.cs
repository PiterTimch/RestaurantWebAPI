using Core.Models.AdminUser;
using Core.Models.Search;
using Core.Models.Search.Params;

namespace Core.Interfaces;

public interface IUserService
{
    Task<List<AdminUserItemModel>> GetAllUsersAsync();
    Task<SearchResult<AdminUserItemModel>> SearchUsersAsync(UserSearchModel model);
}
