using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Features.Posts.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Posts;

[ApiController]
[Route("api/v0/posts")]
public class PostsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPostsController([FromQuery] string? cursor = null, [FromQuery] int take = 10,
        [FromQuery] string? order = null, [FromQuery] bool mine = false)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);
            var isAdmin = User.IsInRole("admin") || User.IsInRole("superadmin");
            var currentUserId = JwtHelper.GetUserIdFromUser(User);

            var query = new GetAllPostQuery(take, orderParams.OrderType, cursorParams.CursorType, orderParams.FieldName,
                cursorParams.CursorId, mine, isAdmin, currentUserId);
            
            var response = await mediator.Send(query);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        
    }

    [HttpGet("{id:guid}", Name = nameof(GetPostByIdController))]
    public async Task<IActionResult> GetPostByIdController([FromRoute] Guid id)
    {
        try
        {
            var isAdmin = User.IsInRole("admin") || User.IsInRole("superadmin");
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var query = new GetPostByIdQuery(id, isAdmin, currentUserId);
            var response = await mediator.Send(query);

            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CreatePostController([FromBody] CreatePostRequestDto createPostRequestDto)
    {
        var userId = JwtHelper.GetUserIdFromUser(User);
        var command = new CreatePostCommand(userId, createPostRequestDto.Title, createPostRequestDto.Description,
            createPostRequestDto.PostStatus);
        
        var response = await mediator.Send(command);
        
        return CreatedAtAction(nameof(GetPostByIdController),
            new { id = response.Id }, response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeletePostController([FromRoute] Guid id)
    {
        try
        {
            var command = new DeletePostCommand(id);
            await mediator.Send(command);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> PatchPostController([FromRoute] Guid id,
        [FromBody] JsonPatchDocument<PatchPostRequestDto> patchPostRequestDto)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var command = new PatchPostCommand(id, patchPostRequestDto, currentUserId);
            var response = await mediator.Send(command);
            
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }
}