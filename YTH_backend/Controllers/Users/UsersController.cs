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
    [HttpGet]
    
    public async Task<IActionResult> GetPersonalDataController()
    {
        var query = new GetPersonalDataQuery();
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserController(Guid id)
    {
        var query = new GetUserQuery(id);
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}/events")]
    [Authorize]
    public async Task<IActionResult> GetUserEventsController(Guid id, [FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

            if (take <= 0)
                take = 10;

            var query = new GetUserEventsQuery(id, take, orderParams.OrderType, cursorParams.CursorType,
                orderParams.FieldName, cursorParams.CursorId);
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

    [HttpPost("{id:guid}/anonymize")]
    [Authorize]
    public async Task<IActionResult> AnonimyzeUserController(Guid id)
    {
        var command = new AnonymizeUserCommand(id);
        throw new NotImplementedException();
    }
    
    [HttpPost("{id:guid}/events/{eventId:guid}")]
    [Authorize]
    public async Task<IActionResult> AddEventsToUserController(Guid id, Guid eventId)
    {
        var command = new AddEventToUserCommand(id, eventId);
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}/events/{eventId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteEventFromUserController(Guid id, Guid eventId)
    {
        var command = new DeleteEventFromUserCommand(id, eventId);
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:guid}/courses/{eventId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteCourseFromUserController(Guid id, Guid eventId)
    {
        var command = new DeleteCourseFromUserCommand(id, eventId);
        throw new NotImplementedException();
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> PatchUserController(Guid id,
        [FromBody] JsonPatchDocument<PatchUserRequestDto> patchUserRequestDto)
    {
        throw new NotImplementedException();
    }
}