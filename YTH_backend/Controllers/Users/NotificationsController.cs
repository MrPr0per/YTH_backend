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
    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetAllNotificationsController([FromRoute] Guid id, [FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        try
        {
            var userIdClaim = User.FindFirst("sub")?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? throw new UnauthorizedAccessException("User ID not found in token");

            var userId = Guid.Parse(userIdClaim);

            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

            var query = new GetAllNotificationsQuery(id, userId, take, orderParams.OrderType, cursorParams.CursorType,
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

    [HttpGet("{notificationId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetNotificationController([FromRoute] Guid id, Guid notificationId)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");
        
        var userId = Guid.Parse(userIdClaim);

        var query = new GetNotificationQuery(id, userId, notificationId);
        throw new NotImplementedException();
    }

    [HttpPatch("{notificationId:guid}/markAsRead")]
    [Authorize]
    public async Task<IActionResult> ReadNotification(Guid notificationId, Guid id)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");
        
        var userId = Guid.Parse(userIdClaim);
        
        var command = new ReadNotificationCommand(id, userId, notificationId);
        throw new NotImplementedException();
    }
}