namespace WebApi.Dtos.Comment;

public class CommentStockDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public string createdBy { get; set; } = string.Empty;
}