namespace Core.Models.Search.Params;

public class PostDepartmentSearchModel
{
    public string? CityName { get; set; }
    public int ItemPerPage { get; set; } = 5;
}
