using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Post;
using YTH_backend.Enums;
using YTH_backend.Features.Posts.Commands;
using YTH_backend.Features.Posts.Queries;

namespace YTH_backend.Controllers.Posts;

[ApiController]
[Route("api/v0/posts")]
public class PostsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpGet()]
    public async Task<IActionResult> GetAllPostsController([FromQuery] int from = 0, [FromQuery] int take = 10,
        [FromQuery] OrderType orderType = OrderType.Asc)
    {
        var query = new GetAllPostQuery(from, take, orderType);
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostByIdController(Guid id)
    {
        var query = new GetPostByIdQuery(id);
        throw new NotImplementedException();
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePostController([FromBody] CreatePostRequestDto createPostRequestDto)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);
        
        var command = new CreatePostCommand(userId, createPostRequestDto.Title, createPostRequestDto.ShortDescription, createPostRequestDto.Description, createPostRequestDto.Status);
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePostController(Guid id)
    {
        var command = new DeletePostCommand(id);
        throw new NotImplementedException();
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PatchPostController(Guid id, [FromBody] JsonPatchDocument<PatchPostRequestDto> patchPostRequestDto)
    {
        var command = new PatchPostCommand(id, patchPostRequestDto);
        throw new NotImplementedException();
    }
}