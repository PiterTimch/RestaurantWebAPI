namespace Core.Models.Search.Params;

public class CitySearchModel
{
    public string? Name { get; set; }
    public int ItemPerPage { get; set; } = 5;
}
