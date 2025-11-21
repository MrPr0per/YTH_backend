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
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
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

    [HttpGet("{registrationId}")]
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
    public async Task<IActionResult> GetUserEventByIdController(Guid registrationId)
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
    
    [HttpDelete("{registrationId}")]
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
    public async Task<IActionResult> DeleteUserEventByIdController(Guid registrationId)
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
    
    //TODO
    // [HttpGet]
    // public async Task<IActionResult> GetAllUserEventsController()
}