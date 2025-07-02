using Core.Models.Search;

namespace Core.Models.Search.Params;


public class UserSearchModel
{
    public string? Role { get; set; }
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; } = 1;
    public int ItemPerPAge { get; set; } = 10;
}
