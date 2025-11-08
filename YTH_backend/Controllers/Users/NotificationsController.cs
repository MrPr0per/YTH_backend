using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.Enums;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Models.User;

namespace YTH_backend.Controllers.Users;

[ApiController]
[Route("api/v0/users/{id:guid}/notifications")]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllNotificationsController([FromRoute] Guid id, [FromQuery] int from = 0, [FromQuery] int take = 10,
        [FromQuery] OrderType orderType = OrderType.Asc)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");
        
        var userId = Guid.Parse(userIdClaim);
        
        var query = new GetAllNotificationsQuery(id, userId, from, take, orderType);
        throw new NotImplementedException();
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