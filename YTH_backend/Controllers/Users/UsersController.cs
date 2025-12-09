using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.DTOs.Event;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models;

namespace YTH_backend.Controllers.Users;

[ApiController]
[Route("api/v0/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserController([FromRoute] Guid id)
    {
        try
        {
            var query = new GetUserQuery(id);
            var response = await mediator.Send(query);
            
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new {error = ex.Message});
        }
    }

    [HttpGet]
    [Authorize(Roles = "superadmin")]
    public async Task<IActionResult> GetAllUsers([FromQuery] string? cursor = null, [FromQuery] int take = 10,
        [FromQuery] string? order = null)
    {
        
        var orderParams = QueryParamsParser.ParseOrderParams(order);
        var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

        

        var query = new GetAllUsersQuery(take, orderParams.OrderType, cursorParams.CursorType,
            orderParams.FieldName, cursorParams.CursorId);
        var response = await mediator.Send(query);
        
        return Ok(response);
    }

    [HttpPost("{id:guid}/anonymize")]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> AnonymizeUserController([FromRoute] Guid id)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);
            
            var command = new AnonymizeUserCommand(id, userId);
            await mediator.Send(command);
            
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> PatchUserController([FromRoute] Guid id,
        [FromBody] JsonPatchDocument<PatchUserRequestDto> patchUserRequestDto)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);
            var command = new PatchUserCommand(id, userId, patchUserRequestDto);
            await mediator.Send(command);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}