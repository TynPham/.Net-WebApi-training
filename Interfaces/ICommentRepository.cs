using WebApi.Dtos.Comment;
using WebApi.Model;

namespace WebApi.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetCommentsAsync();
    Task<Comment?> GetCommentByIdAsync(int id);
    Task<Comment> CreateCommentAsync(CreateCommentRequestDto comment,  string AppUserId);
    Task<Comment> UpdateCommentAsync(UpdateCommentRequestDto comment, int id);
    Task DeleteCommentAsync(int id);
}