namespace WebApi.Extensions;

public class PaginationResults<T>
{
    public List<T> Items { get; set; } = new List<T>();
    
    public int Total { get; set; }
    
    public int Page { get; set; }
    
    public int Limit { get; set; }
    
    public PaginationResults(List<T> items, int total, int page, int limit) {
        Items = items;
        Total = total;
        Page = page;
        Limit = limit;
    }
}