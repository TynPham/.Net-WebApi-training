using System.ComponentModel.DataAnnotations;

namespace WebApi.Helpers;

public class QueryObject
{
    public string? CompanyName { get; set; }
    
    [Required(ErrorMessage = "Page is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1.")]
    public int page { get; set; }
    [Required(ErrorMessage = "Limit is required.")]
    [Range(10, int.MaxValue, ErrorMessage = "Limit must be greater than or equal to 10.")]
    public int limit { get; set; }
}