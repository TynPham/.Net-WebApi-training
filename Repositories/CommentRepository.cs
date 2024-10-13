using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Dtos.Comment;
using WebApi.Interfaces;
using WebApi.Model;

namespace WebApi.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;
    
    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comment>> GetCommentsAsync()
    {
        return await _context.Comments.ToListAsync();
    }
    
    public async Task<Comment> GetCommentByIdAsync(int id)
    {
        return await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Comment> CreateCommentAsync(CreateCommentRequestDto comment)
    {
        var newComment= new Comment
        {
            StockId = comment.StockId,
            Content = comment.Content,
            CreatedOn = comment.CreatedOn,
            Title = comment.Title
        };

        _context.Comments.Add(newComment);
        await _context.SaveChangesAsync();
        return newComment;
    }

    public async Task<Comment> UpdateCommentAsync(UpdateCommentRequestDto comment, int id)
    {
        var existingComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        
        if (existingComment == null)
        {
            throw new Exception("Comment not found");
        }
        
        existingComment.Content = comment.Content;
        existingComment.CreatedOn = comment.CreatedOn;
        existingComment.Title = comment.Title;
        
        await _context.SaveChangesAsync();
        return existingComment;
    }
    
    public async Task DeleteCommentAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}