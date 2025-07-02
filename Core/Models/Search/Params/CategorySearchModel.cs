namespace Core.Models.Search.Params;

public class CategorySearchModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }

    public int ItemsPerPage { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}
