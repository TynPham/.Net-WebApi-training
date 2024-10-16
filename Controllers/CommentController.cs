using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.Comment;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Model;

namespace WebApi.Controllers;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly UserManager<AppUser> _userManager;
    
    public CommentController(ICommentRepository commentRepository, UserManager<AppUser> userManager)
    {
        _commentRepository = commentRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments()
    {
        var comments = await _commentRepository.GetCommentsAsync();
        TypeAdapterConfig<Comment, CommentDto>.NewConfig()
            .Map(dest => dest.createdBy, src => src.AppUser.UserName);
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
           TypeAdapterConfig<Comment, CommentDto>.NewConfig()
            .Map(dest => dest.createdBy, src => src.AppUser.UserName);
        var response = comment.Adapt<CommentDto>();
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequestDto request)
    {
        var userName = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(userName);
        var comment = await _commentRepository.CreateCommentAsync(request, appUser.Id);
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