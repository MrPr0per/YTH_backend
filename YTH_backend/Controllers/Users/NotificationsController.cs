using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.Enums;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.User;

namespace YTH_backend.Controllers.Users;

[ApiController]
[Route("api/v0/users/{id:guid}/notifications")]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> GetAllNotificationsController([FromRoute] Guid id, [FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);

            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

            var query = new GetAllNotificationsQuery(id, currentUserId, take, orderParams.OrderType,
                cursorParams.CursorType,
                orderParams.FieldName, cursorParams.CursorId);

            var response = await mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("{notificationId:guid}")]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> GetNotificationController([FromRoute] Guid id, [FromRoute] Guid notificationId)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var query = new GetNotificationQuery(id, currentUserId, notificationId);
            var response = await mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPatch("{notificationId:guid}/markAsRead")]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> ReadNotification([FromRoute] Guid notificationId, [FromRoute] Guid id)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var command = new ReadNotificationCommand(id, currentUserId, notificationId);
            
            await mediator.Send(command);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}