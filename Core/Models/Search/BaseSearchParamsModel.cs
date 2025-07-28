namespace Core.Models.Search;

public class BaseSearchParamsModel
{
    public int Page { get; set; } = 1;
    public int ItemPerPAge { get; set; } = 10;
}
