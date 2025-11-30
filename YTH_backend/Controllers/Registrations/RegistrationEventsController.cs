using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Event;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Registrations;

[ApiController]
[Route("api/v0/registrations/events")]
public class RegistrationEventsController(IMediator mediator) : ControllerBase
{
    
    [HttpPost]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> AddEventToUserController(AddEventToUserRequestDto request)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);

            var command = new AddEventToUserCommand(request.UserId, request.EventId, currentUserId);
            var response = await mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetUserEventByIdController),
                new { id = response.Id},
                response
                );
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }

    [HttpGet("{registrationId:guid}", Name = nameof(GetUserEventByIdController))]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> GetUserEventByIdController([FromRoute] Guid registrationId)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var query = new GetUserEventByIdQuery(registrationId, currentUserId);
            var response = await mediator.Send(query);

            return Ok(response);
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
    
    [HttpDelete("{registrationId:guid}")]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> DeleteUserEventByIdController([FromRoute] Guid registrationId)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var query = new DeleteEventFromUserCommand(registrationId, currentUserId);
            await mediator.Send(query);
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

    [HttpGet]
    public async Task<IActionResult> GetAllUserEventsController([FromQuery] string? cursor = null,
        [FromQuery] int take = 10, [FromQuery] string? order = null, [FromQuery] Guid? user = null, [FromQuery] Guid? eventId = null)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var isAdmin = User.IsInRole("admin") || User.IsInRole("superadmin");

            var query = new GetUserEventsQuery(user, take, orderParams.OrderType, cursorParams.CursorType,
                orderParams.FieldName, cursorParams.CursorId, eventId, currentUserId, isAdmin);

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
}