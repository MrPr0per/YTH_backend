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
    public async Task<IActionResult> GetAllPostsController([FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

            var query = new GetAllPostQuery(take, orderParams.OrderType, cursorParams.CursorType, orderParams.FieldName,
                cursorParams.CursorId);
            var response = await mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPostByIdController(Guid id)
    {
        var query = new GetPostByIdQuery(id);
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> CreatePostController([FromBody] CreatePostRequestDto createPostRequestDto)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);
        
        var command = new CreatePostCommand(userId, createPostRequestDto.Title, createPostRequestDto.Description, createPostRequestDto.PostStatus);
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> DeletePostController(Guid id)
    {
        var command = new DeletePostCommand(id);
        throw new NotImplementedException();
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> PatchPostController(Guid id, [FromBody] JsonPatchDocument<PatchPostRequestDto> patchPostRequestDto)
    {
        var command = new PatchPostCommand(id, patchPostRequestDto);
        throw new NotImplementedException();
    }
}