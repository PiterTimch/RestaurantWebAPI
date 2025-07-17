namespace Core.Models.Search.Params;

public class PostDepartmentSearchModel
{
    public string? Name { get; set; }
    public int ItemPerPage { get; set; } = 5;
}
