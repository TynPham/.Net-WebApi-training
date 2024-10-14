using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.Comment;
using WebApi.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    
    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetComments()
    {
        var comments = await _commentRepository.GetCommentsAsync();
        var response = comments.Adapt<List<CommentDto>>();
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment.Adapt<CommentDto>());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequestDto request)
    {   
        var comment = await _commentRepository.CreateCommentAsync(request);
        return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment.Adapt<CommentDto>());
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentRequestDto request)
    {
        var comment = await _commentRepository.UpdateCommentAsync(request, id);
        return Ok(comment.Adapt<CommentDto>());
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await _commentRepository.DeleteCommentAsync(id);
        return Ok(true);
    }
}