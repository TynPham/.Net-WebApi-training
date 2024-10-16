using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model;

[Table("Portfolios")]
public class Portfolio
{
    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; } = null;
    
    public int StockId { get; set; }

    public Stock Stock { get; set; } = null;
}